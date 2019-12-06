using System;
using System.IO;

namespace hw2_j_oneill
{
    class BrownianMotion
    {
        //Seeds a Random object upon creation of instance
        //to avoid repeated random values
        Random rand = new Random();

        //Takes a reference to data, an array of doubles and outputs
        //to a file "filename", location within the same directory
        //as the .exe file
        public void outputToFile(string filename, double[] data)
        {
            //Write to file
            StreamWriter sw = new StreamWriter(@filename, false);
            for(int i = 0; i< data.Length; i++)
            {
                //Writes each entry in data on a newline
                //When opened in excel, data will be arranged
                //in a column
                sw.WriteLine(data[i]);
            }
            sw.Close();
        }
        //Creates a single random walk of length n and stores it in data
        public void generateWalk(double[] data, int n, double T, double sigma)
        {
            //Note: T must be > 0
            if(T<0)
            {
                T = 0.0;
            }
            //n is total number of steps
            double dt = T / (double)n;
            double delta = sigma * Math.Sqrt(dt);


            //Start with value of 0
            data[0] = 0;
            //Each step, either add or subtract by delta
            for( int i = 1; i < n; i++)
            {
                //Each step, a virtual coin is flipped
                //If true, add delta
                //If false, subtract delta
                if(Convert.ToBoolean(rand.Next(0, 2)))
                {
                    data[i] = data[i - 1] + delta;
                }
                else
                {
                    data[i] = data[i - 1] - delta;
                }
            }
        }
        //Generate nens random walks, each of length nperwalk.
        //Take the final element of each random walk and store it in array data
        public void ensembledata( double[] data, int nperwalk, int nens, double T, double sigma)
        {
            //For storing a particular random walk
            double[] tempData = new double[nperwalk];
            //Generate nens random walks
            for (int i = 0; i < nens; i++)
            {
                //Generate new random walk with nperwalk entries
                generateWalk(tempData, nperwalk, T, sigma);
                //Save final value from random walk in array
                data[i] = tempData[nperwalk - 1];
            }
        }
        //Calculate the mean and variance of the data generated
        //Note: Two different formulae for variance, so formula
        //for variance of a population was selected
        public void meanvar(double[] data, ref double mean, ref double var)
        {
            //First, find the mean value of data
            double sum = 0;
            double size = (Double)data.Length;

            //Sum up all entries in data
            for(int i = 0; i < data.Length; i++)
            {
                sum += data[i];
            }
            //And divide by number of entries to find mean
            mean = sum / size;

            //Second, calculate the variance of the population
            sum = 0;
            for (int i = 0; i < data.Length; i++)
            {
                sum += (data[i] - mean) * (data[i] - mean);
            }
            var = sum/size;
        }
        
    }
    //Test class, containing the main function
    //Creats object from BrownianMotion and uses it
    //to generate data
    class Test
    {
        static void Main(string[] args)
        {
            BrownianMotion brownian = new BrownianMotion();
            
            
            /*GENERATE SINGLE RANDOM WALK*/
            //Set up variables
            int n = 1000;
            double T = 10.0;
            double sigma = 1.0;
            double[] outputData = new double[n];

            //Run simulation
            brownian.generateWalk(outputData, n, T, sigma);
            //Output to file
            string fileName = "SingleRandomWalk.csv";
            brownian.outputToFile(fileName, outputData);
            //Output to console to alert user
            Console.WriteLine("Generating single random walk.");
            Console.WriteLine("Data saved to {0}", fileName);

            /*GENERATE ENSEMBLE RANDOM WALKS*/
            //Perform for sigma = 1.0
            //Set up variables
            int nperwalk = 1000;
            int nens = 1000;
            double mean = 0;
            double var = 0;
            double[] ensembleData = new double[nens];

            //Generate ensemble data set
            brownian.ensembledata(ensembleData, nperwalk, nens, T, sigma);
            //Calculate mean and variance
            brownian.meanvar(ensembleData, ref mean, ref var);
            //Calculate mean and variance
            brownian.meanvar(ensembleData, ref mean, ref var);
            Console.WriteLine("----------------");
            Console.WriteLine("Generating ensemble data for {0} random walks.", nens);
            Console.WriteLine("T : {0}, SIGMA : {1}", T, sigma);
            Console.WriteLine("MEAN: {0}, VARIANCE: {1}", mean, var);
            //Output to file
            fileName = "EnsembleWalks.csv";
            brownian.outputToFile(fileName, ensembleData);
            Console.WriteLine("Data saved to {0}\n", fileName);

            //Generate ensemble data sets for sigma = 2.0 to sigma = 9.0
            for (sigma = 2.0; sigma < 10.00; sigma++)
            {
                //Generate ensemble data set
                brownian.ensembledata(ensembleData, nperwalk, nens, T, sigma);
                //Calculate mean and variance
                brownian.meanvar(ensembleData, ref mean, ref var);
                //Display results on screen
                Console.WriteLine("-------------------------------------------------------");
                Console.WriteLine("Generating ensemble data for {0} random walks.", nens);
                Console.WriteLine("NPERWALK: {0} ,T : {1}, SIGMA : {2}", nperwalk, T, sigma);
                Console.WriteLine("MEAN: {0}, VARIANCE: {1}\n", mean, var);
            }
            //Give user a chance to read results
            Console.WriteLine("Press Enter to continue ...\n");
            Console.ReadLine();
        }
    }
}
