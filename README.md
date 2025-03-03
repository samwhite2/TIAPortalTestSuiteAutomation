# TIA Portal Test Suite Automation

This WPF Application is used to assist the automation of Test Suite in TIA Portal. The application provides functionalities to:

- Connect to TIA Portal Project
- Open Selected TIA Portal Project
- Import TestCases from .tat file into TIA Portal Project
- Save TestCases to .tat file
- Run Single TestCase
- Run All TestCases
- Write Test Results to a ListBox

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
    - Click the `Connect` button to connect to the TIA Portal project.

2. **Import TestCases**:
    - Enter the path to the .tat file in the `tbTestCasePath` textbox.
    - Click the `Import TestCases` button to import the test cases into the TIA Portal project.

3. **Save TestCases**:
    - Enter the path to save the .tat file in the `tbTestCasePath` textbox.
    - Click the `Save TestCases` button to save the test cases to a .tat file.

4. **Run Single TestCase**:
    - Enter the name of the test case in the `tbTestCase` textbox.
    - Click the `Run TestCase` button to run the selected test case.

5. **Run All TestCases**:
    - Click the `Run All TestCases` button to run all the test cases in the project.

6. **Import PLC Block**:
    - Enter the path to the PLC block file in the `tbBlockFilePath` textbox.
    - Click the `Import Block` button to import the PLC block into the TIA Portal project.

7. **Export PLC Block**:
    - Enter the name of the PLC block in the `tbBlockName` textbox.
    - Click the `Export Block` button to export the PLC block to a file.

8. **Generate and Import OB**:
    - Enter the name of the data block in the `tbIDBName` textbox.
    - Click the `Generate and Import OB` button to generate and import the OB.

## Features

- **Connect to TIA Portal Project**: Connect to a TIA Portal project using the specified project path.
- **Import TestCases**: Import test cases from a .tat file into the TIA Portal project.
- **Save TestCases**: Save test cases to a .tat file.
- **Run Single TestCase**: Run a single test case specified by the user.
- **Run All TestCases**: Run all test cases in the project.
- **Import PLC Block**: Import a PLC block from a file into the TIA Portal project.
- **Export PLC Block**: Export a PLC block to a file.
- **Generate and Import OB**: Generate and import an OB into the TIA Portal project.

## Contributing

Contributions are welcome! Please open an issue or submit a pull request for any improvements or bug fixes.

## License

This project is licensed under the MIT License. S
