# TIA Portal Test Suite Automation

This WPF Application is used to assist the automation of Test Suite in TIA Portal. The application provides functionalities to:

- Connect to TIA Portal Project
- Open Selected TIA Portal Project
- Import TestCases from .tat file into TIA Portal Project
- Save TestCases to .tat file
- Run Single TestCase
- Run All TestCases
- Write Test Results to a ListBox
- Generate OB and Instance DB from a selected FB under test

## Table of Contents

- [Installation](#installation)
- [Usage](#usage)
- [Features](#features)
- [Contributing](#contributing)
- [License](#license)

## Installation

1. Clone the repository:

2. Open the solution in Visual Studio.

3. Restore the NuGet packages.

4. Build the solution.

## Usage

1. **Connect to TIA Portal Project**:
    - Enter the path to the TIA Portal project in the `tbProjectPath` textbox.
    - Click the `Connect to TIA` button to connect to the TIA Portal project.

2. **Import TestCases**:
    - Enter the path to the .tat file in the `tbTestCasePath` textbox.
    - Enter the TestCase name in the `tbTestCase` textbox.
    - Enter the TestCase Scope (PLC Under Test) in the `tbTestCaseScope` textbox.
    - Click the `Import TestCases` button to import the test cases into the TIA Portal project.

3. **Save TestCases**:
    - Enter the name of the test case in the `tbTestCase` textbox.
    - Enter the path to save the .tat file in the `tbTestCasePath` textbox.
    - Click the `Save TestCases` button to save the test cases to a .tat file.

5. **Run Single TestCase**:
    - Enter the name of the test case in the `tbTestCase` textbox.
    - Click the `Run TestCase` button to run the selected test case.

6. **Run All TestCases**:
    - Click the `Run All TestCases` button to run all the test cases in the Application Test Group.

7. **Generate and Import OB**:
    - Enter the name of the FB Under test in the `tbFunctionName` textbox.
    - Enter the name of the data block you want to generate from the selected FB in the `tbIDBName` textbox.
    - Click the `Generate and Import OB` button to generate and import the OB and the iDB.

## Features

- **Connect to TIA Portal Project**: Connect to a TIA Portal project using the specified project path.
- **Import TestCases**: Import test cases from a .tat file into the TIA Portal project.
- **Save TestCases**: Save test cases to a .tat file.
- **Run Single TestCase**: Run a single test case specified by the user.
- **Run All TestCases**: Run all test cases in the project.
- **Generate and Import OB and iDB**: Generate and import an OB and iDB into the TIA Portal project.

## Contributing

Contributions are welcome! Please open an issue or submit a pull request for any improvements or bug fixes.

## License

This project is licensed under the MIT License. S
