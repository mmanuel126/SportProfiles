using System.Diagnostics;
using sportprofiles.Services;
using sportprofiles.ViewModels;
using sportprofiles.Views.Account;
using Syncfusion.Licensing;

namespace sportprofiles;

public partial class App : Application
{
	public App()
	{
		//Register syncfusion license
		SyncfusionLicenseProvider.RegisterLicense("MzUxMjM5NUAzMjM3MmUzMDJlMzBEd1ltR0VNY0lhMTdUZ1B3bEwxOGNZZXhuanFsbEt1bVJXOHNPc2RhVmd3PQ==");
		InitializeComponent();

		AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
		TaskScheduler.UnobservedTaskException += OnUnobservedTaskException!;
	}

	private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
	{
		//Handle exceptions that occur in the UI thread
		if (e.ExceptionObject is Exception ex)
		{
			//log the error - so dev can analyze to fix later
			Console.WriteLine($"Unhandled Exception: {ex.Message}");
			LogError(ex);
			var w = e.IsTerminating;
			
		}
	}

	private void OnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
    {
		// Handle exceptions that occur in non-UI tasks
        var ex = e.Exception;
		//log the error - so dev can analyze to fix later
		Console.WriteLine($"Unhandled Exception: {ex.Message}");
		LogError(ex);

		// Mark the exception as handled to prevent the app from crashing
        e.SetObserved();
	}

	private void LogError(Exception ex)
	{
		// Log the exception, e.g., using a logging library or saving it to a file.
        Console.WriteLine($"Exception: {ex.Message}\nStack Trace: {ex.StackTrace}");
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new LoginPage(new MemberViewModel(new Members())));
	}
}