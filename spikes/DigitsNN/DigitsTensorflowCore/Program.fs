open Tensorflow
open System.IO
open System.Net
open System.Threading
open System.Threading.Tasks
open ICSharpCode.SharpZipLib.GZip
open ICSharpCode.SharpZipLib.Core
open System
open NumSharp
open Tensorflow
open Tensorflow

let inline (!>) (x:^a) : ^b = ((^a or ^b) : (static member op_Implicit : ^a -> ^b) x)

let extractGZip (gzipFileName:string) targetDir =
    let dataBuffer = Array.create 4096 0uy
    let fnOut = Path.Combine(targetDir, Path.GetFileNameWithoutExtension(gzipFileName))
    use fs = new FileStream(gzipFileName, FileMode.Open, FileAccess.Read)
    use gzipStream = new GZipInputStream(fs)
    use fsOut = File.Create(fnOut)
    StreamUtils.Copy(gzipStream, fsOut, dataBuffer)

let download (url:string) destDir destFileName =
    Directory.CreateDirectory destDir |> ignore
    let relativeFilePath = Path.Combine(destDir, destFileName)
    if File.Exists(relativeFilePath) then
        printfn "%s already exists." relativeFilePath
        false
    else
        let wc = new WebClient()
        printfn "Downloading %s" relativeFilePath
        let download = Task.Run(fun _ -> wc.DownloadFile(url, relativeFilePath) |> ignore)
        while not download.IsCompleted do
            Thread.Sleep(1000)
            printf "."
        printfn ""
        printfn "Downloaded %s" relativeFilePath
        true

type DataSetMnist(imgs:NDArray, lbls:NDArray, dtype:TF_DataType, reshape) =
    let mutable epochsCompleted = 0
    let mutable indexInEpoch = 0
    let numExamples = imgs.shape.[0]

    let mutable labels = lbls.astype(dtype.as_numpy_datatype())
    let mutable data =
        let reshaped = imgs.reshape(imgs.shape.[0], imgs.shape.[1] * imgs.shape.[2])
        let typed = reshaped.astype(dtype.as_numpy_datatype())
        let ndArray = !> (1.0f / 255.0f)
        np.multiply(typed, ndArray)

    member this.Data = data
    member this.Labels = labels
    member this.NextBatch batchSize =
        let fakeData, shuffle = false, true
        let mutable start = indexInEpoch
        if epochsCompleted = 0 && start = 0 && shuffle then
            let perm0 = np.arange(numExamples)
            np.random.shuffle(perm0)
            data <- data.[perm0]
            labels <- labels.[perm0]
        if start + batchSize > numExamples then
            epochsCompleted <- epochsCompleted + 1
            let restNumExamples = numExamples - start
            if shuffle then
                let perm = np.arange(numExamples);
                np.random.shuffle(perm);
                data <- data.[perm];
                labels <- labels.[perm]
            start <- 0
            indexInEpoch <- batchSize - restNumExamples
            let _end = indexInEpoch
            let imagesNewPart = data.[np.arange(start, _end)]
            let labelsNewPart = labels.[np.arange(start, _end)]
            imagesNewPart, labelsNewPart
        else
            indexInEpoch <- indexInEpoch + batchSize
            let _end = indexInEpoch
            data.[np.arange(start, _end)], labels.[np.arange(start, _end)]

type MNIST() =
    let DEFAULT_SOURCE_URL = "https://storage.googleapis.com/cvdf-datasets/mnist/"
    let TRAIN_IMAGES = "train-images-idx3-ubyte.gz"
    let TRAIN_LABELS = "train-labels-idx1-ubyte.gz"
    let TEST_IMAGES = "t10k-images-idx3-ubyte.gz"
    let TEST_LABELS = "t10k-labels-idx1-ubyte.gz"

    let read32 (bytestream:FileStream) =
        let buffer = Array.create (sizeof<UInt32>) 0uy
        let _ = bytestream.Read(buffer, 0, 4)
        np.frombuffer(buffer, ">u4").Data<UInt32>(0)

    let extractImages file limit =
        use bytestream = new FileStream(file, FileMode.Open)
        let magic = read32 bytestream
        if magic <> 2051ul then
            failwith (sprintf "Invalid magic number %d in MNIST image file: %s" magic file)
        else
            let numImages =
                let num = read32 bytestream
                match limit with
                | Some l -> Math.Min(num, l)
                | None -> num
            let rows = read32 bytestream
            let cols = read32 bytestream
            let len = int(rows * cols * numImages)
            let buf = Array.create len 0uy
            bytestream.Read(buf, 0, buf.Length) |> ignore
            let data = np.frombuffer(buf, np.uint8)
            data.reshape(int(numImages), int(rows), int(cols), 1)
    let denseToOneHot (labelsDense:NDArray) numClasses =
        let numLabels = labelsDense.shape.[0]
        let labelsOneHot = np.zeros([|numLabels; numClasses|])
        [0..numLabels-1] |> List.iter (fun row ->
            let col = labelsDense.Data<byte>(row)
            labelsOneHot.SetData(1.0, row, int(col))
        )
        labelsOneHot
    let extractLabels file oneHot numClasses limit =
        use bytestream = new FileStream(file, FileMode.Open)
        let magic = read32 bytestream
        if magic <> 2049ul then
            failwith(sprintf "Invalid magic number %d in MNIST label file: %s" magic file)
        else
            let numImages =
                let num = read32 bytestream
                match limit with
                | Some l -> Math.Min(num, l)
                | None -> num
            let len = int(numImages)
            let buf = Array.create len 0uy
            bytestream.Read(buf, 0, buf.Length) |> ignore
            let labels = np.frombuffer(buf, np.uint8)
            if not oneHot then labels
            else denseToOneHot labels numClasses

    member this.ReadDataSets dir oneHot numClasses (validationSize:int) =
        download (DEFAULT_SOURCE_URL + TRAIN_IMAGES) dir TRAIN_IMAGES |> ignore
        extractGZip (Path.Combine(dir, TRAIN_IMAGES)) dir
        let trainImagesFile = Path.Combine(dir, TRAIN_IMAGES.Split('.').[0])
        let trainImages = extractImages trainImagesFile None

        download (DEFAULT_SOURCE_URL + TRAIN_LABELS) dir TRAIN_LABELS |> ignore
        extractGZip (Path.Combine(dir, TRAIN_LABELS)) dir
        let trainLabelsFile = Path.Combine(dir, TRAIN_LABELS.Split('.').[0])
        let trainLabels = extractLabels trainLabelsFile oneHot numClasses None

        download (DEFAULT_SOURCE_URL + TEST_IMAGES) dir TEST_IMAGES |> ignore
        extractGZip (Path.Combine(dir, TEST_IMAGES)) dir
        let testImagesFile = Path.Combine(dir, TEST_IMAGES.Split('.').[0])
        let testImages = extractImages testImagesFile None

        download (DEFAULT_SOURCE_URL + TEST_LABELS) dir TEST_LABELS |> ignore
        extractGZip (Path.Combine(dir, TEST_LABELS)) dir
        let testLabelsFile = Path.Combine(dir, TEST_LABELS.Split('.').[0])
        let testLabels = extractLabels testLabelsFile oneHot numClasses None

        let _end = trainImages.shape.[0]
        let validationSlice = np.arange(validationSize)
        let validationImages = trainImages.[validationSlice]
        let validationLabels = trainLabels.[validationSlice]

        let trainSlice = np.arange(validationSize, _end)
        let trainImgs = trainImages.[trainSlice]
        let trainLbls = trainLabels.[trainSlice]

        let train = new DataSetMnist(trainImgs, trainLbls, TF_DataType.TF_FLOAT, true)
        let validation = new DataSetMnist(validationImages, validationLabels, TF_DataType.TF_FLOAT, true);
        let test = new DataSetMnist(testImages, testLabels, TF_DataType.TF_FLOAT, true);

        train, validation, test

let imgH, imgW, nClasses, epochs, batchSize, h1Simple, learningRate = 28, 28, 10, 10, 100, 200, 0.001f
let imgSizeFlat = imgH * imgW

// Network configuration
// 1st Convolutional Layer
let filterSize1 = 5  // Convolution filters are 5 x 5 pixels.
let numFilters1 = 16 //  There are 16 of these filters.
let stride1 = 1  // The stride of the sliding window

// 2nd Convolutional Layer
let filterSize2 = 5 // Convolution filters are 5 x 5 pixels.
let numFilters2 = 32 // There are 32 of these filters.
let stride2 = 1 // The stride of the sliding window

// Fully-connected layer.
let h1Complex = 128 // Number of neurons in fully-connected layer.

let mutable x:Tensor = null
let mutable y:Tensor = null
let mutable loss:Tensor = null
let mutable accuracy:Tensor = null
let mutable optimizer:Operation = null

let weightVariable name shape =
    let initer = tf.truncated_normal_initializer(stddev = 0.01f)
    tf.get_variable(name, shape = shape, dtype = tf.float32, initializer = initer)

let biasVariable name shape =
    let initial = tf.constant(0.f, shape = shape, dtype = tf.float32)
    tf.get_variable(name, dtype = tf.float32, initializer = initial)

let convLayer (x:Tensor) filterSize numFilters stride (name:string) =
    Python.``with``(
        tf.variable_scope(name),
        fun _ ->
            let numInChannel = x.shape.[x.NDims - 1]
            let W = weightVariable "W" (!> [|filterSize; filterSize; numInChannel; numFilters|])
            let b = biasVariable "b" [|numFilters|]
            let layer = tf.nn.conv2d(x, W, strides = [| 1; stride; stride; 1 |], padding = "SAME")
            tf.nn.relu(layer + (!> b))
    )

let maxPool (x:Tensor) ksize stride name =
    tf.nn.max_pool(x,
        ksize = [|1; ksize; ksize; 1|],
        strides = [| 1; stride; stride; 1 |],
        padding = "SAME",
        name = name)

let flattenLayer (layer:Tensor) =
    Python.``with``(
        tf.variable_scope("Flatten_layer"),
        fun _ ->
            let layerShape = layer.TensorShape
            let slice = new Slice(Nullable(1), Nullable(4))
            let numFeatures = layerShape.[slice].Size
            tf.reshape(layer, [|-1; numFeatures|])
    )

let fcLayer2 (x:Tensor) numUnits (name:string) useRelu =
    Python.``with``(
        tf.variable_scope(name),
        fun _ ->
            let inDim = x.shape.[1]
            let W = weightVariable ("W_" + name) (!> [|inDim; numUnits|])
            let b = biasVariable ("b_" + name) [|numUnits|]
            let layer = tf.matmul(x, (!> W)) + (!> b)
            if useRelu then tf.nn.relu(layer) else layer
    )

let fcLayer (x:Tensor) (numUnits:int) name useRelu =
    let inDim = x.shape.[1]
    let initer = tf.truncated_normal_initializer(stddev = 0.01f)
    let shape = !> [|inDim; numUnits|]
    let W = tf.get_variable("W_" + name, shape, tf.float32, initer)
    let initial = tf.constant(0.f, numUnits)
    let b = tf.get_variable("b_" + name, dtype = tf.float32, initializer = initial)
    let layer = tf.matmul(x, !> W) + (!> b)
    if useRelu then tf.nn.relu(layer) else layer

let prepareData() =
    let train, validation, test = MNIST().ReadDataSets "mnist" true 10 5000
    printfn "Size of:"
    printfn "- Training-set: %d" train.Data.len
    printfn "- Validation-set: %d" validation.Data.len
    printfn "- Test-set: %d" test.Data.len
    train, validation, test

let buildSimpleGraph() =
    let graph = (new Graph()).as_default()
    x <- tf.placeholder(tf.float32, !> [|-1; imgSizeFlat|], "X")
    y <- tf.placeholder(tf.float32, !> [|-1; nClasses|], "Y")

    let fc1 = fcLayer x h1Simple "FC1" true
    let outputLogits = fcLayer fc1 nClasses "OUT" false
    let logits = tf.nn.softmax_cross_entropy_with_logits(labels = y, logits = outputLogits)
    loss <- tf.reduce_mean(logits, name = "loss")
    optimizer <- tf.train.AdamOptimizer(learning_rate = learningRate, name = "Adam-op").minimize(loss)
    let correctPrediction = tf.equal(tf.argmax(outputLogits, 1), tf.argmax(y, 1), name = "correct_pred")
    accuracy <- tf.reduce_mean(tf.cast(correctPrediction, tf.float32), name = "accuracy")
    let clsPrediction = tf.argmax(outputLogits, 1, "predictions")
    graph

let buildComplexGraph() =
    let graph = (new Graph()).as_default()
    Python.``with``(
        tf.name_scope("Input"),
        fun _ ->
            x <- tf.placeholder(tf.float32, shape = (!> [|-1; imgH; imgW; 1|]), name = "X");
            y <- tf.placeholder(tf.float32, shape = (!> [|-1; nClasses|]), name = "Y")
    )

    let conv1 = convLayer x filterSize1 numFilters1 stride1 "conv1"
    let pool1 = maxPool conv1 2 2 "pool1"
    let conv2 = convLayer pool1 filterSize2 numFilters2 stride2 "conv2"
    let pool2 = maxPool conv2 2 2 "pool2"
    let layerFlat = flattenLayer pool2
    let fc1 = fcLayer2 layerFlat h1Complex "FC1" true
    let outputLogits = fcLayer2 fc1 nClasses "OUT" false

    Python.``with``(
        tf.variable_scope("Train"),
        fun _ ->
            Python.``with``(
                tf.variable_scope("Loss"),
                fun _ -> loss <- tf.reduce_mean(tf.nn.softmax_cross_entropy_with_logits(y, outputLogits), name = "loss")
            ) |> ignore
            Python.``with``(
                tf.variable_scope("Optimizer"),
                fun _ -> optimizer <- tf.train.AdamOptimizer(learningRate, "Adam-op").minimize(loss)
            ) |> ignore
            Python.``with``(
                tf.variable_scope("Accuracy"),
                fun _ ->
                    let correctPrediction = tf.equal(tf.argmax(outputLogits, 1), tf.argmax(y, 1), name = "correct_pred")
                    accuracy <- tf.reduce_mean(tf.cast(correctPrediction, tf.float32), name = "accuracy")
            ) |> ignore
            Python.``with``(
                tf.variable_scope("Prediction"),
                fun _ ->
                    let clsPrediction = tf.argmax(outputLogits, 1, "predictions")
                    clsPrediction
            ) |> ignore
    ) |> ignore
    graph

let randomize (x:NDArray) (y:NDArray) =
    let perm = np.random.permutation(y.shape.[0])
    np.random.shuffle(perm)
    x.[perm], y.[perm]

let getNextBatch (x:NDArray) (y:NDArray) start _end =
    let xBatch = x.[sprintf "%d:%d" start _end]
    let yBatch = y.[sprintf "%d:%d" start _end]
    xBatch, yBatch

let reformat (x:NDArray) (y:NDArray) =
    let (img_size, num_ch, num_class) = (np.sqrt(!> x.shape.[1]), 1, Python.len(np.unique<int>(np.argmax(y, 1))))
    let dataset = x.reshape(x.shape.[0], !> img_size, !> img_size, num_ch).astype(np.float32)
    dataset, y

let train (sess:Session) (train:DataSetMnist) (validation:DataSetMnist) =
    let numTrIter = train.Labels.len / batchSize
    let init = tf.global_variables_initializer()
    sess.run init |> ignore

    let mutable lossVal = 100.0f
    let mutable accuracyVal = 0.f

    let x_train, y_train = reformat train.Data train.Labels

    [0..epochs-1] |> List.iter (fun epoch ->
        printfn "Training epoch: %d" (epoch + 1)
        let xTrain, yTrain = randomize x_train y_train

        [0..numTrIter-1] |> List.iter (fun iteration ->
            let start = iteration * batchSize
            let _end = (iteration + 1) * batchSize
            let xBatch, yBatch = getNextBatch xTrain yTrain start _end
            sess.run(optimizer, new FeedItem(x, xBatch), new FeedItem(y, yBatch)) |> ignore

            if iteration % 100 = 0 then
                let result = sess.run([|loss; accuracy|], new FeedItem(x, xBatch), new FeedItem(y, yBatch))
                lossVal <- !> result.[0]
                accuracyVal <- !> result.[1]
                printfn "iter %s: Loss=%s, Training Accuracy=%s"
                    (iteration.ToString("000")) (lossVal.ToString("0.0000")) (accuracyVal.ToString("P"))            
        )
        let x_valid, y_valid = reformat validation.Data validation.Labels
        let results1 = sess.run([|loss; accuracy|], new FeedItem(x, x_valid), new FeedItem(y, y_valid))
        lossVal <- !> results1.[0]
        accuracyVal <- !> results1.[1]
        printfn "---------------------------------------------------------"
        printfn "Epoch: %d, validation loss: %s, validation accuracy: %s"
            (epoch + 1) (lossVal.ToString("0.0000")) (accuracyVal.ToString("P"))
        printfn "---------------------------------------------------------"
    )

let test (sess:Session) (test:DataSetMnist) =
    let x_test, y_test = reformat test.Data test.Labels
    let result = sess.run([|loss; accuracy|], new FeedItem(x, x_test), new FeedItem(y, y_test))
    let lossTest:float32 = !> result.[0]
    let accuracyTest:float32 = !> result.[1]
    printfn "---------------------------------------------------------"
    printfn "Test loss: %s, test accuracy: %s" (lossTest.ToString("0.0000")) (accuracyTest.ToString("P"))
    printfn "---------------------------------------------------------"

let run() =
    let training, validation, testing = prepareData()
    //buildSimpleGraph() |> ignore
    buildComplexGraph() |> ignore

    Tensorflow.Python.``with``(
        tf.Session(),
        fun sess ->
            train sess training validation
            test sess testing
    )

[<EntryPoint>]
let main argv =
    run()
    0
