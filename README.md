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

#####Testing HttpClient in UILogic.Tests
[HowTo](http://stackoverflow.com/questions/13360309/using-fiddler-with-windows-store-unit-test)
LoopbackExempt	OneMSQFT.UILogic.Tests

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


Dependencies
-----
[Install Player Framework Version 1.3.2 from this link](http://playerframework.codeplex.com/releases/view/116468)

Deployment
-----

- Debug > Runs Store Application in Debug Mode
- Debug Kiosk > Runs Kiosk Application in Debug Mode
- Release > Runs Store Application in Release Mode
- Release Kiosk > Runs Kiosk Application in Release Mode

Application Life Cycle
-----
###Launch

####Extended Splash
- Load Site Data
    - Check Memory Cache
    - Check Network Connectivity
    - Check Cache if no connectivity
    - Call Repository if connectivity
    - Cache return data in memory and disk

####Launch Navigation and/or [Activate](http://msdn.microsoft.com/en-us/library/windows/apps/xx130647.aspx#Activating) Navigation
- Check Navigation for Deep Link Item (Event or Exhibit)
- Check Configuration for Startup Item (Event or Exhibit)
-   If Item Found
    -   If Event
        -   Navigate to TimelinePage(eventId)
    -   If Exhibit
        -   Navigate to ExhibitDetailsPage(exhibitId)
-   Else, Navigate to TimelinePage()

###Resume
-   If Kiosk Mode
    - Regard a Resume as an Inactivity Reset
    - Check Configuration for Startup Item (Event or Exhibit)
    -   If Item Found
        -   If Event
            -   Navigate to TimelinePage(eventId)
        -   If Exhibit
            -   Navigate to ExhibitDetailsPage(exhibitId)
    -   Else, Navigate to TimelinePage()
-   Else
    -  Restore Session State and Resume Current Page
