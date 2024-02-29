# ReverseStringApp - Installation & Usage Guide

## Installation Steps

### Prerequisites
- **Operating System:** Windows 10, version 1809 (October 2018 Update) or later. Windows 11 is also supported.
- **PowerShell:** Ensure PowerShell is installed and updated to the latest version available for your system.
- **.NET Framework:** Version 4.8 or later.

### Installation Procedure
1. Navigate to the installation package directory, e.g., `C:\ReverseStringApp\WapProjTemplate1\AppPackages\WapProjTemplate1_1.0.2.0_Debug_Test`.
2. Locate the "Install" script. Right-click on it and select "Run with PowerShell" to start the installation process.
3. Wait for the installation to complete. You may see progress indicators or prompts during this process.
4. After installation, the application "WapProjTemplate1" will be available for launch. Find and click on it to start the application.

### Troubleshooting
If you encounter any issues during installation, please contact [tian.xun.ng@hp.com](mailto:tian.xun.ng@hp.com) for support.

## Functionality

- **Input Box:** Users can input the string they wish to reverse.
- **Process Button:** Submits the entered text to the backend for reversal.
- **Backend Processing:** Utilizes a full trust WRP process to reverse the string.
- **Output Display:** A read-only text box displays the reversed string.
- **Randomize Button:** Shuffles the input string in a random order.
- **Clear Button:** Clears all text from the input and output text boxes.

## Design

- **User Interface:** Utilizes a Universal Windows Platform (UWP) for user interaction.
- **Backend Processing:** A full trust WRP process operates in the background, initiated by the UWP front-end.
- **Integration:** A packaging solution combines both UWP and WRP components for seamless operation.

### Communication Architecture
- Main communication between UWP front-end and WRP backend is via AppServices, hosted within the UWP's `App.xaml`.
- AppServices API facilitates communication without engaging in backend computations, with alternative IPC methods available if needed.
- Upon launch, the WRP process connects to the UWP's AppService, which then maintains the connection for subsequent requests.
- The WRP process lifecycle is tied to the UWP AppService connection, ensuring no orphaned processes remain.

## Test Scenarios

- **Basic Functionality:** Verify correct reversal and display of input strings.
- **Randomization:** Ensure effective and non-repetitive shuffling of the input string.
- **Clear Functionality:** Confirm that all text boxes are correctly cleared.
- **Error Handling:** Test the application's response to invalid inputs and edge cases.
- **Performance:** Assess application performance under high load or with large input strings.
- **Stability:** Ensure application stability over extended use, including proper background process management.
