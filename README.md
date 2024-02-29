# ReverseStringApp

## Introduction

ReverseStringApp is a versatile application designed to manipulate string inputs by users in various ways, including reversing the string, randomizing the characters, and clearing inputs and outputs for a fresh start. It's built using a Universal Windows Platform (UWP) interface for seamless user interaction, with a robust backend processing using a full trust WPF.

## Table of Contents

- [Installation](#installation)
  - [Prerequisites](#prerequisites)
  - [Installation Procedure](#installation-procedure)
- [Usage](#usage)
  - [Functionality](#functionality)
  - [Design](#design)
  - [Communication Architecture](#communication-architecture)
- [Troubleshooting](#troubleshooting)
- [Test Scenarios](#test-scenarios)
- [Contact](#contact)

## Installation

### Prerequisites

- **Operating System:** Windows 10, version 1809 (October 2018 Update) or later. Windows 11 is also supported.
- **PowerShell:** Ensure PowerShell is installed and updated to the latest version available for your system.
- **.NET Framework:** Version 4.8 or later.
- **Developer mode:** Ensure developer mode is enabled, or just run the Install script which will guide you through the installation process.

### Installation Procedure

1. Navigate to the installation package directory, e.g., `C:\ReverseStringApp\WapProjTemplate1\AppPackages\WapProjTemplate1_1.0.2.0_Debug_Test`.
2. Locate the "Install" script within the folder. Right-click on it and select "Run with PowerShell" to initiate the installation process.
3. Allow some time for the installation to complete. You may see progress indicators or prompts during this process.
4. Once installation is finished, the application "WapProjTemplate1" will be available for launch. Locate and click on it to start using the application.

## Usage

### Functionality

- **Input:** A text box is provided for users to enter the string they wish to process.
- **Process Button:** A dedicated button is available to submit the entered text to the backend for processing.
- **Backend Processing:** Utilizes a full trust WPF to reverse the string.
- **Output Display:** A read-only text box displays the processed result, ensuring users cannot modify it directly.
- **Randomize Function:** An additional button is included to shuffle the input string in a random order.
- **Clear Function:** A clear button is available to erase all text from the input and output text boxes, resetting the application state.

### Design

- **User Interface:** The application employs a Universal Windows Platform (UWP) interface for user interaction.
- **Backend Processing:** A full trust capability WPF process operates in the background, initiated by the UWP front-end.
- **Integration:** A packaging solution combines both the UWP and WPF components, ensuring seamless operation.

### Communication Architecture

- The primary communication channel between the UWP front-end and the WPF backend is through AppServices, hosted within the UWP's App.xaml file. This background process facilitates communication without engaging in backend computations.
- Alternative inter-process communication (IPC) methods such as named pipes, sockets, or file I/O can be utilized, but AppServices is chosen for its simplicity and integration with UWP.
- Upon application launch, the WPF process initiates a connection to the UWP's AppService. The UWP then maintains this connection for subsequent requests, such as sending input strings to the WPF for processing.
- The lifecycle of the WPF process is tied to the UWP AppService connection. Closing this connection will also terminate the WPF process to prevent orphaned processes.
- Requests sent from the UWP to the WPF are handled efficiently, ensuring responsive application performance.

## Test Scenarios

- **Basic Functionality Test:** Verify that input strings are correctly reversed and displayed in the output text box.
- **Randomization Test:** Ensure the randomize button effectively shuffles the input string in a non-repetitive manner.
- **Clear Function Test:** Confirm that the clear button resets both the input and output text boxes.
- **Error Handling:** Test the application's response to invalid inputs, such as excessively long strings or unsupported characters.
- **Performance:** Assess the application's performance under high load, such as rapidly repeated requests or large input strings.
- **Stability:** Confirm the application's stability over extended periods of use, including the proper management and termination of background processes.

## Contact

For further assistance or to report issues, please email tian.xun.ng@hp.com.
