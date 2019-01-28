using System;
using System.IO;
using System.Diagnostics;
using System.Windows.Data;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArconGreasepaint
{
    public enum WindSpeedEnum
    {
        mph=1,
        mps=2,
        knots=3
    }

    public enum ReleaseTypeEnum
    {
        ground=1,
        vent=2,
        stack=3
    }

    public class ComparisonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value?.Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value?.Equals(true) == true ? parameter : Binding.DoNothing;
        }
    }
    public class ArconFile : Notifier
    {
        public enum ResultCode
        {
            DoesNotExist,
            ReadOnly,
            Exists,
            ReadFailure,
            OK
        }
        private FileInfo fileInfo;
        public string OutputFile { get; set; }
        public string CfdFileName { get; set; }
        public string[] MetFileNames { get; set; }

        public int? NumberOfMetFiles { get; set; }
        public double? LowerMeasurmentHeight { get; set; }
        public double? UpperMeasurementHeight { get; set; }
        public double? ReleaseHeight { get; set; }
        public double? BuildingArea { get; set; }
        public double? VerticalVelocity { get; set; }
        public double? StackFlow { get; set; }
        public double? StackRadius { get; set; }
        public double? DistanceToReceptor { get; set; }
        public double? ReceptorHeight { get; set; }
        public double? ElevationDifference { get; set; }
        public double? DirectionToSource { get; set; }

        // these items will have default values because they are associated with radio buttons
        public WindSpeedEnum WindSpeedDataType { get; set; }
        public ReleaseTypeEnum ReleaseType { get; set; }

        // this will have a default value, just because
        public char ExpandedOutputFlag { get; set; }
        // the following have default values
        public double SurfaceRoughnessLength { get; set; }
        public double WindDirectionWindow { get; set; }
        public double MinimumWindSpeed { get; set; }
        public double AveragingSectorWidthConstant { get; set; }
        public double Sigy0 { get; set; }
        public double Sigz0 { get; set; }
        const Int32 intervalCount = 10;
        public Int32[] AveragingIntervals { get; set; }
        public Int32[] MinHours { get; set; }

        public ArconFile(string longNewFileName)
        {
            // this constructor is used when only a new file is requested
            fileInfo = new FileInfo(longNewFileName);
            // initialize parameters
            AveragingIntervals = new Int32[intervalCount];
            MinHours = new Int32[intervalCount];
            WindSpeedDataType = WindSpeedEnum.mps;
            ReleaseType = ReleaseTypeEnum.ground;
            SetAllDefaults();
            MetFileNames = new string[] { "","","","","","","","","",""};
            NumberOfMetFiles = 1;
            ExpandedOutputFlag = 'n';
        }

        public ArconFile(string longOldFileName, string longNewFileName) : this(longNewFileName)
        {
            ReadFile(longOldFileName);
        }
        private void SetAllDefaults()
        {
            SetDefaultRoughnessLength();
            SetDefaultWindDirectionWindow();
            SetDefaultMinWindSpeed();
            SetDefaultSectorWidthConstant();
            SetDefaultInitDiffSigY();
            SetDefaultInitDiffSigZ();
            SetDefaultAveragingIntervals();
            SetDefaultminHours();

            // clear other fields;
            OutputFile = "";
            CfdFileName = "";
            MetFileNames = new string[0];
        }

        private void SetDefaultRoughnessLength()
        {
            SurfaceRoughnessLength = 0.1;
        }

        private void SetDefaultWindDirectionWindow()
        {
            WindDirectionWindow = 90;
        }

        private void SetDefaultMinWindSpeed()
        {
            MinimumWindSpeed = 0.5;
        }

        private void SetDefaultSectorWidthConstant()
        {
            AveragingSectorWidthConstant = 4.0;
        }

        private void SetDefaultInitDiffSigY()
        {
            Sigy0 = 0.0;
        }

        private void SetDefaultInitDiffSigZ()
        {
            Sigz0 = 0.0;
        }

        private void SetDefaultAveragingIntervals()
        {
            AveragingIntervals = new Int32[] { 1, 2, 4, 8, 12, 24, 96, 168, 360, 720 };
        }
        private void SetDefaultminHours()
        {
            MinHours = new Int32[] { 1, 2, 4, 8, 11, 22, 89, 152, 324, 648 };
        }

        private bool HasWriteAccessToFolder(DirectoryInfo currentDirectory)
        {
            try
            {
                // Attempt to get a list of security permissions from the folder. 
                // This will raise an exception if the path is read only or do not have access to view the permissions. 
                System.Security.AccessControl.DirectorySecurity ds = currentDirectory.GetAccessControl();
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        private bool ReadFile(string longOldFileName)
        {
            // Objectives:
            // 1) set the current working directory
            // 2) open a StreamReader to the specified file name (must be an existing file)
            // 3) read in the data
            // 4) close the StreamWriter

            try
            {
                //Pass the file path and file name to the StreamReader constructor
                StreamReader sr = new StreamReader(longOldFileName);

                // Read the data - GO!
                NumberOfMetFiles = Convert.ToInt32(sr.ReadLine().Substring(0, 2));
                for (int i = 0; i < (int)NumberOfMetFiles; i++)
                {
                    MetFileNames[i] = sr.ReadLine().Substring(0, 40).Trim();
                }
                LowerMeasurmentHeight = Convert.ToDouble(sr.ReadLine().Substring(0, 9));
                UpperMeasurementHeight = Convert.ToDouble(sr.ReadLine().Substring(0, 9));

                WindSpeedDataType = (WindSpeedEnum)Convert.ToInt32(sr.ReadLine().Substring(0, 4));
                ReleaseType = (ReleaseTypeEnum)Convert.ToInt32(sr.ReadLine().Substring(0, 4));

                ReleaseHeight = Convert.ToDouble(sr.ReadLine().Substring(0, 9));
                BuildingArea = Convert.ToDouble(sr.ReadLine().Substring(0, 9));
                VerticalVelocity = Convert.ToDouble(sr.ReadLine().Substring(0, 9));
                StackFlow = Convert.ToDouble(sr.ReadLine().Substring(0, 9));
                StackRadius = Convert.ToDouble(sr.ReadLine().Substring(0, 9));

                string jumbo = sr.ReadLine();
                DirectionToSource = Convert.ToInt32(jumbo.Substring(0, 4));
                WindDirectionWindow = Convert.ToInt32(jumbo.Substring(4, 4));

                DistanceToReceptor = Convert.ToDouble(sr.ReadLine().Substring(0, 9));
                ReceptorHeight = Convert.ToDouble(sr.ReadLine().Substring(0, 9));
                ElevationDifference = Convert.ToDouble(sr.ReadLine().Substring(0, 9));

                OutputFile = sr.ReadLine().Substring(0, 40).Trim();
                CfdFileName = sr.ReadLine().Substring(0, 40).Trim();
                SurfaceRoughnessLength = Convert.ToDouble(sr.ReadLine().Substring(0, 4));
                MinimumWindSpeed = Convert.ToDouble(sr.ReadLine().Substring(0, 9));
                AveragingSectorWidthConstant = Convert.ToDouble(sr.ReadLine().Substring(0, 9));

                jumbo = sr.ReadLine();
                for (int i = 0; i < intervalCount; i++)
                {
                    AveragingIntervals[i] = Convert.ToInt32(jumbo.Substring(i * 4, 4));
                }
                jumbo = sr.ReadLine();
                for (int i = 0; i < intervalCount; i++)
                {
                    MinHours[i] = Convert.ToInt32(jumbo.Substring(i * 4, 4));
                }
                jumbo = sr.ReadLine();
                Sigy0 = Convert.ToDouble(jumbo.Substring(0, 9));
                Sigz0 = Convert.ToDouble(jumbo.Substring(9, 9));

                ExpandedOutputFlag = sr.ReadLine()[0];
                sr.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool WriteFile()
        {
            // Objectives:
            // 1) set the current working directory
            // 2) open a StreamWriter to the specified file name (may or may not be an existing file)
            // 3) write out the data
            // 4) close the StreamWriter

            // assume all fields have been screened for errors by the input panels
            StreamWriter sw = new StreamWriter(fileInfo.FullName);

            sw.WriteLine(NumberOfMetFiles.ToString().PadLeft(2));
            int trueNumber = Convert.ToInt32(NumberOfMetFiles);
            for (int i = 0; i < trueNumber; i++)
            {
                sw.WriteLine(MetFileNames[i].PadRight(40));
            }
            sw.WriteLine("{0,9:F2}", LowerMeasurmentHeight);
            sw.WriteLine("{0,9:F2}", UpperMeasurementHeight);
            sw.WriteLine("{0,4:F0}", (int) WindSpeedDataType);
            sw.WriteLine("{0,4:F0}", (int) ReleaseType);
            sw.WriteLine("{0,9:F2}", ReleaseHeight);
            sw.WriteLine("{0,9:F2}", BuildingArea);
            sw.WriteLine("{0,9:F2}", VerticalVelocity);
            sw.WriteLine("{0,9:F2}", StackFlow);
            sw.WriteLine("{0,9:F2}", StackRadius);
            sw.WriteLine("{0,4}{1,4}", DirectionToSource, WindDirectionWindow);
            sw.WriteLine("{0,9:F2}", DistanceToReceptor);
            sw.WriteLine("{0,9:F2}", ReceptorHeight);
            sw.WriteLine("{0,9:F2}", ElevationDifference);

            sw.WriteLine(OutputFile.PadRight(40));
            sw.WriteLine(CfdFileName.PadRight(40));

            sw.WriteLine("{0,4:#.###}", SurfaceRoughnessLength);
            sw.WriteLine("{0,9:F2}", MinimumWindSpeed);
            sw.WriteLine("{0,9:F2}", AveragingSectorWidthConstant);

            string jumbo = "";
            for (int i = 0; i < intervalCount; i++)
            {
                jumbo += String.Format("{0,4:F0}", AveragingIntervals[i]);
            }
            sw.WriteLine(jumbo);
            jumbo = "";
            for (int i = 0; i < intervalCount; i++)
            {
                jumbo += String.Format("{0,4:F0}", MinHours[i]);
            }
            sw.WriteLine(jumbo);

            sw.WriteLine("{0,9:F2}{1,9:F2}",Sigy0, Sigz0);

            sw.WriteLine(ExpandedOutputFlag);

            sw.Close();

            return true;
        }

        public int RunFile()
        {
            // Objectives:
            // 1) set the current working directory
            // 3) create a new process to run the arcon96f.exe program with the input file name as a command line argument
            // 5) wait for the process to end

            //Directory.SetCurrentDirectory(fileInfo.DirectoryName);
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                CreateNoWindow = false,
                UseShellExecute = false,
                FileName = "arcon96f.exe",
                Arguments = fileInfo.Name,
                WindowStyle = ProcessWindowStyle.Hidden,
                WorkingDirectory = fileInfo.DirectoryName
            };
            try
            {
                Process exeProcess = Process.Start(startInfo);
                exeProcess.WaitForExit();
                return exeProcess.ExitCode;
            }
            catch
            {
                return 1;
            }
        }
    }
}
