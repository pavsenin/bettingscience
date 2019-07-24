open Accord.Neuro.Networks
open Accord.Neuro.ActivationFunctions
open Accord.Neuro.Learning
open Accord.Neuro
open System
open System.Windows.Media.Imaging
open System.IO

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

    let extract text =
        let bitmap = new Bitmap(32, 32, PixelFormat.Format32bppRgb)
        let lines = text.Split([|"\n"|], StringSplitOptions.RemoveEmptyEntries)
        //for (int i = 0; i < 32; i++)
        //{
        //    for (int j = 0; j < 32; j++)
        //    {
        //        if (lines[i][j] == '0')
        //            bitmap.SetPixel(j, i, Color.White);
        //        else bitmap.SetPixel(j, i, Color.Black);
        //    }
        //}
        bitmap

    public double[] ToFeatures(Bitmap bmp)
    {
        double[] features = new double[32 * 32];
        for (int i = 0; i < 32; i++)
            for (int j = 0; j < 32; j++)
                features[i * 32 + j] = (bmp.GetPixel(j, i).R > 0) ? 0 : 1;

        return features;
    }

    let extractSample database buffer label =
        let bitmap = Extract(new String(buffer))
        let image = bitmap.ToBitmapImage()

        let features = ToFeatures(bitmap)
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
                let sample = extractSample buffer label
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

[<EntryPoint>]
let main argv = 
    let network = new DeepBeliefNetwork(new BernoulliFunction(), 1024, 50, 10) // TODO WTF?
    let database = new DigitsDatabase(IsNormalized = false)
    let weights = new GaussianWeights(network) // TODO WTF?
    weights.Randomize() // TODO WTF?
    network.UpdateVisibleWeights() // TODO WTF?
    let teacher = new BackPropagationLearning(network, LearningRate = 0.1, Momentum = 0.9) // TODO WTF?
    //let (inputs, outputs) = Main.Database.Training.GetInstances()
    0


        //        ComputeAccuracy(Main.Database.Testing);
        //        ComputeAccuracy(Main.Database.Training);

        //    }).Start();
        //}

        //void ComputeAccuracy(ObservableCollection<Sample> set) {
        //    double[][] testInputs, testOutputs;
        //    set.GetInstances(out testInputs, out testOutputs);
        //    int total = 0, correct = 0;
        //    for(var i = 0; i < testInputs.Length; i++) {
        //        double[] predicted = Main.Network.GenerateOutput(testInputs[i]);
        //        predicted.Max(out var predValue);
        //        testOutputs[i].Max(out var value);
        //        total++;
        //        if(predicted[value] > 0)
        //            correct++;
        //    }
        //    var accuracy = (1.0 * correct) / total;
        //    Debug.WriteLine($"Total {total} Correct {correct} Accuracy {accuracy}");
        //}