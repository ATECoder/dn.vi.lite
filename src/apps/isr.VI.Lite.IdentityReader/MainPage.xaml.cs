namespace isr.VI.Lite.IdentityReader;

public partial class MainPage : ContentPage
{
    private int _count;

	public MainPage()
	{
        this.InitializeComponent();
	}

	private void OnCounterClicked(object sender, EventArgs e)
	{
        this._count++;

		this.CounterBtn.Text = this._count == 1 ? $"Clicked {this._count} time" : $"Clicked {this._count} times";

        string command = "*IDN?";
        string ipAddress = "192.168.0.154";
        string portNumber = "5025";

        this.WelcomeLabel.Text = $"{this._count} sending device query sync: {command}";
        string response = QueryDevice( ipAddress, portNumber, command );
        this.WelcomeLabel.Text = $"{this._count} sync: {response}";

        this.WelcomeLabel.Text = $"{this._count} sending device query async: {command}";
        response = QueryDeviceAsync( ipAddress, portNumber, command );
        this.WelcomeLabel.Text = $"{this._count} async {response}";

        // this does no work at this time.
#if false

        var session = new Session( ipAddress, int.Parse( portNumber ) );

        this.WelcomeLabel.Text = $"{this._count} sending device query sync: {command}";
        response = QueryDevice( session, command );
        this.WelcomeLabel.Text = $"{this._count} sync: {response}";

        this.WelcomeLabel.Text = $"{this._count} sending device query async: {command}";
        response = QueryDeviceAsync( session, command );
        this.WelcomeLabel.Text = $"{this._count} async {response}";
#endif

        SemanticScreenReader.Announce( this.CounterBtn.Text);
	}


    private static string QueryDevice( string ipAddress, string portNumber, string command )
    {
        try
        {
            var session = new Session( ipAddress, int.Parse( portNumber ) );
            var response = session.Query( command );
            return response;
        }
        catch ( ApplicationException ex )
        {
            Console.WriteLine( ex.ToString() );
        }
        return "Exception occurred";
    }

    private static string QueryDeviceAsync( string ipAddress, string portNumber, string command )
    {
        try
        {
            var session = new Session( ipAddress, int.Parse( portNumber ) );
            var response = session.QueryAsync( command ).Result;
            return response;
        }
        catch ( ApplicationException ex )
        {
            Console.WriteLine( ex.ToString() );
        }
        return "Exception occurred";
    }

#if false

    private static string QueryDevice( Session session, string command )
    {
        try
        {
            var response = session.Query( command );
            return response;
        }
        catch ( ApplicationException ex )
        {
            Console.WriteLine( ex.ToString() );
        }
        return "Exception occurred";
    }

    private static string QueryDeviceAsync( Session session, string command )
    {
        try
        {
            var response = session.QueryAsync( command ).Result;
            return response;
        }
        catch ( ApplicationException ex )
        {
            Console.WriteLine( ex.ToString() );
        }
        return "Exception occurred";
    }

#endif

}

