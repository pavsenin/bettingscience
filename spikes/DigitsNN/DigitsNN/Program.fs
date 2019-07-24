open Accord.Neuro.Networks
open Accord.Neuro.ActivationFunctions
open Accord.Neuro.Learning
open Accord.Neuro
open Accord.Math
open System
open System.Windows.Media.Imaging
open System.IO
open System.Drawing
open System.Drawing.Imaging

type Sample(database) =
    member val Database:DigitsDatabase = database with get, set
    member val Features:double array = [||] with get, set
    member val Image:BitmapImage = null with get, set
    member val Reconstruction:BitmapImage = null with get, set
    member val Result = -1 with get, set
    member val Class = -1 with get, set

    member this.Match =
        if this.Result = -1 then None
        else Some(this.Result = this.Class)

and DigitsDatabase() =
    let mean = Array.zeroCreate 1024
    let dev = Array.create 1024 1

    let extract (text:string) =
        let bitmap = new Bitmap(32, 32, PixelFormat.Format32bppRgb)
        let lines = text.Split([|"\n"|], StringSplitOptions.RemoveEmptyEntries)
        [0..31] |> List.iter (fun i ->
            [0..31] |> List.iter (fun j ->
                let line = lines.[i]
                if line.[j] = '0' then bitmap.SetPixel(j, i, Color.White)
                else bitmap.SetPixel(j, i, Color.Black)
            )
        )
        bitmap

    let toFeatures (bmp:Bitmap) =
        let features = Array.create (32 * 32) 0.
        [0..31] |> List.iter (fun i ->
            [0..31] |> List.iter (fun j ->
                features.[i * 32 + j] <- if bmp.GetPixel(j, i).R > 0uy then 0. else 1.
            )
        )
        features

    let toBitmapImage (bitmap:Bitmap) =
        use memory = new MemoryStream()
        bitmap.Save(memory, ImageFormat.Bmp)
        memory.Position <- 0L
        let bitmapImage = new BitmapImage()
        bitmapImage.BeginInit()
        bitmapImage.StreamSource <- memory
        bitmapImage.CacheOption <- BitmapCacheOption.OnLoad
        bitmapImage.EndInit()
        bitmapImage.Freeze()
        bitmapImage

    let extractSample database (buffer:char array) label =
        let bitmap = extract(new String(buffer))
        let image = toBitmapImage bitmap

        let features = toFeatures bitmap
        let classLabel = Int32.Parse(label)
        new Sample(database, Features = features, Image = image, Class = classLabel, Result = -1)

    member this.Classes = 10
    member val IsNormalized = false with get, set
    member val Training: Sample array = [||] with get, set
    member val Testing: Sample array = [||] with get, set

    member this.Load() =
        let input = File.ReadAllText("digits.txt")
        let reader = new StringReader(input)
        let buffer = Array.create ((32 + 1) * 32) Char.MinValue
        let mutable count = 0

        let mutable isBreak = false
        while not isBreak do
            let read = reader.ReadBlock(buffer, 0, buffer.Length)
            let label = reader.ReadLine()
            if read < buffer.Length || label = null then
                isBreak <- true
            else
                let sample = extractSample this buffer label
                if count < 1000 then
                    this.Training <- Array.append this.Training [|sample|]
                else
                    this.Testing <- Array.append this.Testing [|sample|]
                count <- count + 1

        if this.IsNormalized then
            //let training = this.Training.GetInstances()

            //mean <- training.Mean(dimension: 0)
            //dev <- training.StandardDeviation()

            //let testing = this.Testing.GetInstances()

            //Normalize(training)
            //Normalize(testing)
            ()

//    public void Normalize(double[][] inputs)
//    {
//        for (int i = 0; i < inputs.Length; i++)
//        {
//            for (int j = 0; j < inputs[i].Length; j++)
//            {
//                inputs[i][j] -= mean[j];

//                if (dev[j] != 0)
//                    inputs[i][j] /= dev[j];
//            }
//        }
//    }

//    public void Normalize(double[] inputs)
//    {
//        for (int j = 0; j < inputs.Length; j++)
//        {
//            inputs[j] -= mean[j];

//            if (dev[j] != 0)
//                inputs[j] /= dev[j];

//        }
//    }

//    public Bitmap ToBitmap(double[] features)
//    {
//        if (features.Length != 1024)
//            throw new Exception();

//        Bitmap bitmap = new Bitmap(32, 32, PixelFormat.Format32bppRgb);

//        for (int i = 0; i < 32; i++)
//        {
//            for (int j = 0; j < 32; j++)
//            {
//                int c = i * 32 + j;
//                double v = (features[c] * dev[c]) + mean[c];
//                v = v.Scale(0, 1, 255.0, 0.0);
//                bitmap.SetPixel(j, i, Color.FromArgb((int)v, (int)v, (int)v));
//            }
//        }

//        return bitmap;
//    }
//    }


let getInstances (set:Sample array) =
    let input = Array.create set.Length [||]
    let output = Array.create set.Length [||]
    [0..input.Length-1] |> List.iter (fun i ->
        input.[i] <- set.[i].Features
        let out = Array.create set.[i].Database.Classes 0.
        out.[set.[i].Class] <- 1.
        output.[i] <- out
    )
    (input, output)

let computeAccuracy (network:DeepBeliefNetwork) set =
    let testInputs, testOutputs = getInstances set
    let mutable total, correct = 0, 0
    [0..testInputs.Length-1] |> List.iter (fun i ->
        let predicted = network.GenerateOutput(testInputs.[i])
        let _, predIndex = predicted.Max()
        let _, index = testOutputs.[i].Max()
        total <- total + 1
        if predIndex = index then
            correct <- correct + 1
    )
    let accuracy = float(correct) / float(total)
    printfn "Total %d Correct %d Accuracy %2f" total correct accuracy

[<EntryPoint>]
let main argv = 
    let database = new DigitsDatabase(IsNormalized = false)
    database.Load()
    let network = new DeepBeliefNetwork(new BernoulliFunction(), 1024, 50, 10) // TODO WTF?
    let weights = new GaussianWeights(network) // TODO WTF?
    weights.Randomize() // TODO WTF?
    network.UpdateVisibleWeights() // TODO WTF?
    let teacher = new BackPropagationLearning(network, LearningRate = 0.1, Momentum = 0.9) // TODO WTF?
    let (inputs, outputs) = getInstances database.Training
    [0..200] |> List.iter(fun i ->
        let error = teacher.RunEpoch(inputs, outputs) // TODO WTF?
        printfn "Epoch %d Error %3f" (i + 1) error
    )
    network.UpdateVisibleWeights() // TODO WTF?
    computeAccuracy network database.Training
    computeAccuracy network database.Testing
    0