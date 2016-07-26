# PDFEditorSDKWindowsService
PDF Editor SDK Windows Service Example

To make the sample working do the following steps:

1) Download and install PDF-XChange Editor SDK http://www.tracker-software.com/product/pdf-xchange-editor-sdk

2) Build the WindowsService1 project.

3) Open Developer Command Prompt with *Administrator Priviledges* and go to the project bin directory where the PDFEditorSDKWindowsService.exe lies.

4) Execute "installutil PDFEditorSDKWindowsService.exe" command to register the service. 

5) Open Task Manager -> go to the Services tab -> click Open Services to open the Services management

6) Look for the PDFEditorSDKWindowsService.

7) Click Start to start the service.

8) Wait for the delay time that you've set (for testing wait for 20 seconds).

9) Click Stop to stop the service.

10) Open the directory from which you've installed the service with the PDFEditorSDKWindowsService.exe file. There shold be a non-empty ServiceStart.txt and ServiceStop.txt files.

11) Open the ServiceStart.txt - it should contain the number of pages in the test document.

12) Open the ServiceStop.txt - it should contain the "Success" word.

13) To uninstall a service use the "installutil /u PDFEditorSDKWindowsService.exe" command.
