/**************************************************************************************************
                                    Applied Integration
---------------------------------------------------------------------------------------------------
Module Name: TIA Portal Test Suite Automation

Description: 
This WPF Application is used to assist the automation of Test Suite in TIA Portal. 
The function is used to:

    - Connect to TIA Portal Project
    - Open Selected TIA Portal Project
    - Import TestCases from .tat file into TIA Portal Project
    - Save TestCases to .tat file
    - Run Single TestCase
    - Run All TestCases
    - Write Test Results to a ListBox

---------------------------------------------------------------------------------------------------
 
 Version History
 Date          Engineer      Description
 19/02/2025    SW            Initial Release
***************************************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml;
using System.Xml.Linq;
using Siemens.Engineering;
using Siemens.Engineering.HW;
using Siemens.Engineering.HW.Features;
using Siemens.Engineering.SW;
using Siemens.Engineering.SW.Blocks;
using Siemens.Engineering.TestSuite;
using Siemens.Engineering.TestSuite.ApplicationTest;

namespace TestSuiteAutomation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Global Variable declarations
        private FileInfo projectPath;
        private TiaPortal tiaPortal;
        private Project tiaProject;
        private TestSuiteService testSuiteService;
        private TestCaseExecutor testCaseExecutor;
        private TestCaseComposition testCaseComposition;
        private ApplicationTestSystemGroup applicationTestSystemGroup;
        private TestCase testCase;
        private TestResults testResults;
        private ProjectBase projectBase;
        private PlcSoftware plcSoftware;
        public MainWindow()
        {
            // Initialize Component Start-up
            InitializeComponent();
        }

        private void connectToTIA(Project project)
        {
            // Try to connect to TIA Portal
            try
            {
                // Get the project path from the TextBox
                projectPath = new FileInfo(tbProjectPath.Text);
                var fileName = projectPath.FullName;
                // Create a new TIA Portal instance and open the project
                tiaPortal = new TiaPortal(TiaPortalMode.WithUserInterface);
                tiaProject = tiaPortal.Projects.Open(new FileInfo(fileName));
                testSuiteService = tiaProject.GetService<TestSuiteService>();
                tiaPortal.GetCurrentProcess();

                // Display a message in the ListBox
                lbImportMessage.Items.Insert(0, DateTime.Now.ToString() + tiaProject.Name + " Opened");
                lbImportMessage.Items.Insert(0, DateTime.Now.ToString() + " Connected to TIA Portal Project: " + tiaProject.Path);
            }
            // Catch any exceptions and display the error message in the ListBox
            catch (Exception ex)
            {
                lbImportMessage.Items.Insert(0, DateTime.Now.ToString() + " Failed to connect to TIA Portal Project: " + ex.Message);
            }
        }

        public void LoadFromFile(FileInfo path)
        {
            // Try to load the TestCases from the specified file
            try
            {
                // Get the first project in the TIA Portal
                projectBase = tiaPortal.Projects[0];
                FileInfo LoadFile = new FileInfo(tbTestCasePath.Text);
                testSuiteService = projectBase.GetService<TestSuiteService>();
                testCaseComposition = testSuiteService.ApplicationTestGroup.TestCases;
                // Load the TestCases from the specified file
                testCaseComposition.LoadFromFile(LoadFile, ImportOptions.Override, TCLoadOptions.IgnoreInvalidObject);

                var device = projectBase.Devices[0];
                var deviceItem = device.DeviceItems.First(e => e.Classification == DeviceItemClassifications.CPU && e.Name == tbTestCaseScope.Text);
                var softwareContainer = deviceItem.GetService<SoftwareContainer>();
                // Check if the SoftwareContainer and PlcSoftware are not null
                if (softwareContainer != null && softwareContainer.Software is PlcSoftware plcSoftware)
                {
                    testCase = testSuiteService.ApplicationTestGroup.TestCases.Find(tbTestCase.Text);
                    testCase.SetScope(plcSoftware);
                }
                // Display a message in the ListBox
                else
                {
                    lbMessage.Items.Insert(0, DateTime.Now.ToString() + " Failed to set scope: SoftwareContainer or PlcSoftware is null.");
                }

                lbMessage.Items.Insert(0, DateTime.Now.ToString() + " TestCases Imported from: " + LoadFile);
            }
            // Catch any exceptions and display the error message in the ListBox
            catch (Exception ex)
            {
                lbMessage.Items.Insert(0, DateTime.Now.ToString() + " Failed to load TestCases: " + ex.Message);
            }
        }

        public void SaveToFile(FileInfo path)
        {
            // Try to save the TestCases to the specified file
            try
            {
                // Get the first project in the TIA Portal
                projectBase = tiaPortal.Projects[0];
                // Get the TestSuiteService from the project
                testCase = testSuiteService.ApplicationTestGroup.TestCases.Find(tbTestCase.Text);
                testCase.SaveToFile(new FileInfo(tbTestCasePath.Text));
            }
            // Catch any exceptions and display the error message in the ListBox
            catch (Exception ex)
            {
                lbMessage.Items.Insert(0, DateTime.Now.ToString() + " Failed to save TestCases: " + ex.Message);
            }
        }
        #region generate .xml template for OB
        private void GenerateOBXml(string dataBlockName, string outputPath)
        {
            // Define the XML namespaces
            XNamespace ns = "http://www.siemens.com/automation/Openness/SW/Interface/v5";
            XNamespace sclNs = "http://www.siemens.com/automation/Openness/SW/NetworkSource/StructuredText/v3";

            // Define the XML structure for the OB
            XDocument obXml = new XDocument(
                new XDeclaration("1.0", "utf-8", null),
                new XElement("Document",
                    new XElement("Engineering", new XAttribute("version", "V17")),
                    new XElement("DocumentInfo",
                        new XElement("Created", DateTime.UtcNow.ToString("o")),
                        new XElement("ExportSetting", "WithDefaults"),
                        new XElement("InstalledProducts",
                            new XElement("Product",
                                new XElement("DisplayName", "Totally Integrated Automation Portal"),
                                new XElement("DisplayVersion", "V17 Update 8")
                            ),
                            new XElement("OptionPackage",
                                new XElement("DisplayName", "TIA Portal Openness"),
                                new XElement("DisplayVersion", "V17 Update 8")
                            ),
                            new XElement("OptionPackage",
                                new XElement("DisplayName", "TIA Portal Version Control Interface"),
                                new XElement("DisplayVersion", "V17")
                            ),
                            new XElement("OptionPackage",
                                new XElement("DisplayName", "TIA Portal Test Suite Advanced"),
                                new XElement("DisplayVersion", "V17 Update 1")
                            ),
                            new XElement("Product",
                                new XElement("DisplayName", "STEP 7 Professional"),
                                new XElement("DisplayVersion", "V17 Update 8")
                            ),
                            new XElement("OptionPackage",
                                new XElement("DisplayName", "STEP 7 Safety"),
                                new XElement("DisplayVersion", "V17 Update 6")
                            ),
                            new XElement("Product",
                                new XElement("DisplayName", "WinCC Advanced"),
                                new XElement("DisplayVersion", "V17 Update 8")
                            )
                        )
                    ),
                    new XElement("SW.Blocks.OB", new XAttribute("ID", "0"),
                        new XElement("AttributeList",
                            new XElement("AutoNumber", "true"),
                            new XElement("HeaderAuthor"),
                            new XElement("HeaderFamily"),
                            new XElement("HeaderName"),
                            new XElement("HeaderVersion", "0.1"),
                            new XElement("Interface",
                                new XElement(ns + "Sections",
                                    new XElement("Section", new XAttribute("Name", "Input"),
                                        new XElement("Member", new XAttribute("Name", "Initial_Call"), new XAttribute("Datatype", "Bool"), new XAttribute("Accessibility", "Public"), new XAttribute("Informative", "true"),
                                            new XElement("Comment",
                                                new XElement("MultiLanguageText", new XAttribute("Lang", "en-US"), "Initial call of this OB")
                                            )
                                        ),
                                        new XElement("Member", new XAttribute("Name", "Remanence"), new XAttribute("Datatype", "Bool"), new XAttribute("Accessibility", "Public"), new XAttribute("Informative", "true"),
                                            new XElement("Comment",
                                                new XElement("MultiLanguageText", new XAttribute("Lang", "en-US"), "=True, if remanent data are available")
                                            )
                                        )
                                    ),
                                    new XElement("Section", new XAttribute("Name", "Temp")),
                                    new XElement("Section", new XAttribute("Name", "Constant"))
                                )
                            ),
                            new XElement("IsIECCheckEnabled", "false"),
                            new XElement("MemoryLayout", "Optimized"),
                            new XElement("Name", "Main"),
                            new XElement("Number", "1"),
                            new XElement("ProgrammingLanguage", "SCL"),
                            new XElement("SecondaryType", "ProgramCycle"),
                            new XElement("SetENOAutomatically", "false")
                        ),
                        new XElement("ObjectList",
                            new XElement("MultilingualText", new XAttribute("ID", "1"), new XAttribute("CompositionName", "Comment"),
                                new XElement("ObjectList",
                                    new XElement("MultilingualTextItem", new XAttribute("ID", "2"), new XAttribute("CompositionName", "Items"),
                                        new XElement("AttributeList",
                                            new XElement("Culture", "en-US"),
                                            new XElement("Text")
                                        )
                                    )
                                )
                            ),
                            new XElement("SW.Blocks.CompileUnit", new XAttribute("ID", "3"), new XAttribute("CompositionName", "CompileUnits"),
                                new XElement("AttributeList",
                                    new XElement("NetworkSource",
                                        new XElement(sclNs + "StructuredText",
                                            new XElement("Access", new XAttribute("Scope", "Call"), new XAttribute("UId", "21"),
                                                new XElement("CallInfo", new XAttribute("UId", "22"), new XAttribute("BlockType", "FB"), new XAttribute("Name", dataBlockName),
                                                    new XElement("Instance", new XAttribute("Scope", "GlobalVariable"), new XAttribute("UId", "23"),
                                                        new XElement("Component", new XAttribute("Name", dataBlockName), new XAttribute("UId", "24"))
                                                    ),
                                                    new XElement("Token", new XAttribute("Text", "("), new XAttribute("UId", "25")),
                                                    new XElement("Token", new XAttribute("Text", ")"), new XAttribute("UId", "37"))
                                                )
                                            ),
                                                new XElement("Token", new XAttribute("Text", ";"), new XAttribute("UId", "38")),
                                                new XElement("NewLine", new XAttribute("Num", "1"), new XAttribute("UId", "39"))
                                        )
                                    ),
                                    new XElement("ProgrammingLanguage", "SCL")
                                ),
                                new XElement("ObjectList",
                                    new XElement("MultilingualText", new XAttribute("ID", "4"), new XAttribute("CompositionName", "Comment"),
                                        new XElement("ObjectList",
                                            new XElement("MultilingualTextItem", new XAttribute("ID", "5"), new XAttribute("CompositionName", "Items"),
                                                new XElement("AttributeList",
                                                    new XElement("Culture", "en-US"),
                                                    new XElement("Text")
                                                )
                                            )
                                        )
                                    ),
                                    new XElement("MultilingualText", new XAttribute("ID", "6"), new XAttribute("CompositionName", "Title"),
                                        new XElement("ObjectList",
                                            new XElement("MultilingualTextItem", new XAttribute("ID", "7"), new XAttribute("CompositionName", "Items"),
                                                new XElement("AttributeList",
                                                    new XElement("Culture", "en-US"),
                                                    new XElement("Text")
                                                )
                                            )
                                        )
                                    )
                                )
                            ),
                            new XElement("MultilingualText", new XAttribute("ID", "8"), new XAttribute("CompositionName", "Title"),
                                new XElement("ObjectList",
                                    new XElement("MultilingualTextItem", new XAttribute("ID", "9"), new XAttribute("CompositionName", "Items"),
                                        new XElement("AttributeList",
                                            new XElement("Culture", "en-US"),
                                            new XElement("Text", "\"Main Program Sweep (Cycle)\"")
                                        )
                                    )
                                )
                            )
                        )
                    )
                )
            );
            // Save the XML document to the specified output path with formatting
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                NewLineOnAttributes = false
            };

            using (XmlWriter writer = XmlWriter.Create(outputPath, settings))
            {
                obXml.Save(writer);
            }
        }
        #endregion
        private void ImportPLCBlockFromFile(string filePath)
        {
            // Try to import the PLC block from the specified file
            try
            {
                // Get the first project in the TIA Portal
                projectBase = tiaPortal.Projects[0];
                // Get the PlcSoftware from the project
                plcSoftware = projectBase.Devices[0].DeviceItems[1].GetService<SoftwareContainer>().Software as PlcSoftware;
                PlcBlockGroup plcBlockGroup = plcSoftware.BlockGroup;
                IList<PlcBlock> newBlock = plcBlockGroup.Blocks.Import(new FileInfo(filePath), ImportOptions.Override);
                // Display a message in the ListBox
                lbImportMessage.Items.Insert(0, DateTime.Now.ToString() + " " + newBlock[0].Name + " Block Imported from: " + filePath);
                // Create an instance DB
                plcSoftware.BlockGroup.Blocks.CreateInstanceDB(tbIDBName.Text, true, 5, tbFunctionName.Text);
                // Display a message in the ListBox
                lbImportMessage.Items.Insert(0, DateTime.Now.ToString() + " " + tbIDBName.Text + " InstanceDB Created");
            }
            // Catch any exceptions and display the error message in the ListBox
            catch (Exception ex)
            {
                lbImportMessage.Items.Insert(0, DateTime.Now.ToString() + " Failed to import PLC block from file: " + ex.Message);
            }
        }
        public TestResults Run(TestCase mytestCase)
        {
            // Try to run the specified TestCase
            try
            {
                // Get the first project in the TIA Portal
                projectBase = tiaPortal.Projects[0];
                // Get the TestSuiteService from the project
                testSuiteService = projectBase.GetService<TestSuiteService>();
                testCaseExecutor = testSuiteService.ApplicationTestGroup.GetService<TestCaseExecutor>();
                // Get the TestCase from the TestSuiteService
                mytestCase = testSuiteService.ApplicationTestGroup.TestCases.Find(tbTestCase.Text);
                // Run the specified TestCase
                lbMessage.Items.Insert(0, DateTime.Now.ToString() + " Running TestCase: " + mytestCase.Name);
                return testCaseExecutor.Run(mytestCase);
            }
            // Catch any exceptions and display the error message in the ListBox
            catch (Exception ex)
            {
                lbMessage.Items.Insert(0, DateTime.Now.ToString() + " Failed to run TestCase: " + ex.Message);
                return null;
            }
        }

        public IEnumerable<TestResults> RunAllTests(ApplicationTestSystemGroup allTestCases)
        {
            // Try to run all TestCases
            try
            {
                // Get the first project in the TIA Portal
                projectBase = tiaPortal.Projects[0];
                // Get the TestSuiteService from the project
                testSuiteService = projectBase.GetService<TestSuiteService>();
                testCaseExecutor = testSuiteService.ApplicationTestGroup.GetService<TestCaseExecutor>();
                // Get all the TestCases from the TestSuiteService
                allTestCases = testSuiteService.ApplicationTestGroup;
                List<TestResults> allTestResults = new List<TestResults>();
                // Run all the TestCases
                foreach (var testCase in allTestCases.TestCases)
                {
                    testResults = testCaseExecutor.Run(testCase);
                    allTestResults.Add(testResults);

                    lbMessage.Items.Insert(0, DateTime.Now.ToString() + " Running TestCase: " + testCase.Name);
                }

                return allTestResults;
            }
            // Catch any exceptions and display the error message in the ListBox
            catch (Exception ex)
            {
                lbMessage.Items.Insert(0, DateTime.Now.ToString() + " Failed to run all TestCases: " + ex.Message);
                return null;
            }
        }

        public void WriteResultsForAllTestsToMessageBox(IEnumerable<TestResults> allTestResults)
        {
            // Try to write the results of all TestCases to the ListBox
            try
            {
                foreach (var testResults in allTestResults)
                {
                    // Write the results of each TestCase to the ListBox
                    lbMessage.Items.Insert(0, "*----------------------------------------------------*");
                    lbMessage.Items.Insert(0, DateTime.Now.ToString() + " State: " + testResults.State);
                    lbMessage.Items.Insert(0, DateTime.Now.ToString() + " Warning Count: " + testResults.WarningCount);
                    lbMessage.Items.Insert(0, DateTime.Now.ToString() + " Error Count: " + testResults.ErrorCount);
                    lbMessage.Items.Insert(0, "*----------------------------------------------------*");
                }
            }
            // Catch any exceptions and display the error message in the ListBox
            catch (Exception ex)
            {
                lbMessage.Items.Insert(0, DateTime.Now.ToString() + " Failed to write results: " + ex.Message);
            }
        }

        public void WriteResultsToMessageBox(TestResults testResults)
        {
            // Try to write the results of the specified TestCase to the ListBox
            try
            {
                // Write the results of the specified TestCase to the ListBox
                lbMessage.Items.Insert(0, "*----------------------------------------------------*");
                lbMessage.Items.Insert(0, DateTime.Now.ToString() + " State: " + testResults.State);
                lbMessage.Items.Insert(0, DateTime.Now.ToString() + " Warning Count: " + testResults.WarningCount);
                lbMessage.Items.Insert(0, DateTime.Now.ToString() + " Error Count: " + testResults.ErrorCount);
                lbMessage.Items.Insert(0, "*----------------------------------------------------*");
            }
            // Catch any exceptions and display the error message in the ListBox
            catch (Exception ex)
            {
                lbMessage.Items.Insert(0, DateTime.Now.ToString() + " Failed to write results: " + ex.Message);
            }
        }

        #region Button Click Events
        private void btnRunTest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                testResults = Run(testCase);
                WriteResultsToMessageBox(testResults);
            }
            catch (Exception ex)
            {
                lbMessage.Items.Insert(0, DateTime.Now.ToString() + " Failed to run test: " + ex.Message);
            }
        }

        private void btnRunAllTests_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WriteResultsForAllTestsToMessageBox(RunAllTests(applicationTestSystemGroup));
            }
            catch (Exception ex)
            {
                lbMessage.Items.Insert(0, DateTime.Now.ToString() + " Failed to run all tests: " + ex.Message);
            }
        }

        private void btnConnectToTIA_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                connectToTIA(tiaProject);
            }
            catch (Exception ex)
            {
                lbMessage.Items.Insert(0, DateTime.Now.ToString() + " Failed to connect to TIA: " + ex.Message);
            }
        }

        private void btnExportTest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveToFile(new FileInfo(tbTestCasePath.Text));
            }
            catch (Exception ex)
            {
                lbMessage.Items.Insert(0, DateTime.Now.ToString() + " Failed to export test: " + ex.Message);
            }
        }

        private void btnImportTest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadFromFile(new FileInfo(tbTestCasePath.Text));
            }
            catch (Exception ex)
            {
                lbMessage.Items.Insert(0, DateTime.Now.ToString() + " Failed to import test: " + ex.Message);
            }
        }
        private void btnGenerateAndImportOB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string dataBlockName = tbIDBName.Text;
                string outputPath = @"C:\Users\white.sam\Desktop\Allchange Transfer\blockImport\OB1.xml";
                GenerateOBXml(dataBlockName, outputPath);
                ImportPLCBlockFromFile(outputPath);
            }
            catch (Exception ex)
            {
                lbImportMessage.Items.Insert(0, DateTime.Now.ToString() + " Failed to generate and import OB: " + ex.Message);
            }
        }
        #endregion
    }
}
