
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

        System.Text.StringBuilder builder = new();
        var session = new Session( ipAddress, int.Parse( portNumber ) );
        string response = QueryDevice( session, command );
        _ = builder.Append( $"{this._count}.a: {(string.IsNullOrEmpty( response ) ? "\n" : response)}" );

        System.Threading.Thread.Sleep( 10 );
        session = new Session( ipAddress, int.Parse( portNumber ) );
        response = QueryDevice( session, command );
        _ = builder.Append( $"{this._count}.b: {(string.IsNullOrEmpty( response ) ? "\n" : response)}" );

        System.Threading.Thread.Sleep( 10 );
        response = QueryDevice( session, command );
        _ = builder.Append( $"{this._count}.c: {(string.IsNullOrEmpty( response ) ? "\n" : response)}" );
        this.WelcomeLabel.Text = builder.ToString();

        SemanticScreenReader.Announce( this.CounterBtn.Text);
	}

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

}

