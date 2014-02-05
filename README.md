1MSQFT
======

Windows Store App and future Windows Phone application for 1MSQFT

OneMSQFT.Windows_TemporaryKey
Password: weareratio


### Project Structure


#### Microsoft.Practices.Prism.*
Included reference library for [Developing a Windows Store business app using C#, XAML, and Prism for the Windows Runtime](http://msdn.microsoft.com/en-us/library/windows/apps/xx130643.aspx)

#### OneMSQFT.Portable
PCL used to share Business Logic, String Resources, API, Models, Interface Definitions, and other common code accross Windows and WindowsPhone

#### OneMSQFT.Windows
Windows Store Application

#### OneMSQFT.UILogic
Windows Store App Library to house implementation of ViewModels, Entity View Models, Interface Implementations, etc.


TDD Guidance
-----

#### OneMSQFT.UILogic.Tests
[Testing and deploying Windows Store business apps using C#, XAML, and Prism](http://msdn.microsoft.com/en-us/library/windows/apps/xx130654.aspx)

-   Apply the same folder structure as OneMSQFT.UILogic
-   Name each Class under test with the [Class]Fixture.cs suffix
-   Decorate class with [TestClass]
    -   Decorate methods with [TestMethod]
        -   Methods can return Task and use async/await
-   Mocks
   -   Implementation of Services Interfaces

Design Guidance
-----

- Create shared interface for I[PageName]PageViewModel in OneMSQFT.UILogic.Interfaces.ViewModels
- Implement Design[PageName]PageViewModel extending I[PageName]PageViewModel in OneMSQFT.Windows.DesignViewModels
    - Place required Bindable properties in I[PageName]PageViewModel in OneMSQFT.UILogic.ViewModels
    - Required Bindable properties will generate from UI development
    - Required Business Logic will generate from a combination of Requirements and UI development
        - Create necessary unit tests that Fail in the associated ViewModelFixture class
- Implement [PageName]PageViewModel extending I[PageName]PageViewModel in OneMSQFT.UILogic
    - Test the PageViewModel from a [PageName]PageViewModelFixture in OneMSQFT.UILogic.Tests.ViewModels
    - Develop code to pass unit tests generated from UI development and Requirements

[OneMSQFT].Windows.App.xaml.cs reads a compile time DESIGN constant to instruct the ViewModelLocator to search in OneMSQFT.Windows.DesignViewModels rather than OneMSQFT.UILogic.ViewModels

