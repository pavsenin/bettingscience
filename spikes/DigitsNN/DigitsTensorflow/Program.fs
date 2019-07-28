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

let inline (!>) (x:^a) : ^b = ((^a or ^b) : (static member op_Implicit : ^a -> ^b) x)

let extractGZip gzipFileName targetDir =
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

let imgH, imgW, nClasses, epochs, batchSize, h1, learningRate = 28, 28, 10, 10, 100, 200, 0.001f
let imgSizeFlat = imgH * imgW
let mutable x:Tensor = null
let mutable y:Tensor = null
let mutable loss:Tensor = null
let mutable accuracy:Tensor = null
let mutable optimizer:Operation = null

let fcLayer (x:Tensor) (numUnits:int) name use_relu =
    let inDim = x.shape.[1]
    let initer = tf.truncated_normal_initializer(stddev = 0.01f)
    let shape = !> [|inDim; numUnits|]
    let W = tf.get_variable("W_" + name, shape, tf.float32, initer)
    let initial = tf.constant(0.f, numUnits)
    let b = tf.get_variable("b_" + name, dtype = tf.float32, initializer = initial)
    let layer = tf.matmul(x, !> W) + (!> b)
    if use_relu then tf.nn.relu(layer) else layer

let prepareData() =
    let train, validation, test = MNIST().ReadDataSets "mnist" true 10 5000
    printfn "Size of:"
    printfn "- Training-set: %d" train.Data.len
    printfn "- Validation-set: %d" validation.Data.len
    printfn "- Test-set: %d" test.Data.len
    train, validation, test

let buildGraph() =
    let graph = (new Graph()).as_default()
    x <- tf.placeholder(tf.float32, !> [|-1; imgSizeFlat|], "X")
    y <- tf.placeholder(tf.float32, !> [|-1; nClasses|], "Y")

    let fc1 = fcLayer x h1 "FC1" true
    let outputLogits = fcLayer fc1 nClasses "OUT" false
    let logits = tf.nn.softmax_cross_entropy_with_logits(labels = y, logits = outputLogits)
    loss <- tf.reduce_mean(logits, name = "loss")
    optimizer <- tf.train.AdamOptimizer(learning_rate = learningRate, name = "Adam-op").minimize(loss)
    let correctPrediction = tf.equal(tf.argmax(outputLogits, 1), tf.argmax(y, 1), name = "correct_pred")
    accuracy <- tf.reduce_mean(tf.cast(correctPrediction, tf.float32), name = "accuracy")
    let clsPrediction = tf.argmax(outputLogits, 1, "predictions")
    ()

let randomize (x:NDArray) (y:NDArray) =
    let perm = np.random.permutation(y.shape.[0])
    np.random.shuffle(perm)
    x.[perm], y.[perm]

let getNextBatch (x:NDArray) (y:NDArray) start _end =
    let xBatch = x.[sprintf "%d:%d" start _end]
    let yBatch = y.[sprintf "%d:%d" start _end]
    xBatch, yBatch

let train (sess:Session) (train:DataSetMnist) (validation:DataSetMnist) =
    let numTrIter = train.Labels.len / batchSize
    let init = tf.global_variables_initializer()
    sess.run init |> ignore

    let mutable lossVal = 100.0f
    let mutable accuracyVal = 0.f

    for epoch in Python.range(epochs) do
        printfn "Training epoch: %d" (epoch + 1)
        let xTrain, yTrain = randomize train.Data train.Labels

        for iteration in Python.range(numTrIter) do
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
        let results1 = sess.run([|loss; accuracy|], new FeedItem(x, validation.Data), new FeedItem(y, validation.Labels))
        lossVal <- !> results1.[0]
        accuracyVal <- !> results1.[1]
        printfn "---------------------------------------------------------"
        printfn "Epoch: %d, validation loss: %s, validation accuracy: %s"
            (epoch + 1) (lossVal.ToString("0.0000")) (accuracyVal.ToString("P"))
        printfn "---------------------------------------------------------"

let test (sess:Session) (test:DataSetMnist) =
    let result = sess.run([|loss; accuracy|], new FeedItem(x, test.Data), new FeedItem(y, test.Labels))
    let lossTest:float32 = !> result.[0]
    let accuracyTest:float32 = !> result.[1]
    printfn "---------------------------------------------------------"
    printfn "Test loss: %s, test accuracy: %s" (lossTest.ToString("0.0000")) (accuracyTest.ToString("P"))
    printfn "---------------------------------------------------------"

let run() =
    let training, validation, testing = prepareData()
    buildGraph()

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
