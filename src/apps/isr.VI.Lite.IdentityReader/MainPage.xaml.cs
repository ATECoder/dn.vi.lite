
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
        string portNumber = "5025";
        Dictionary<string, (int ReadAfterWriteDelay, int InterQqueryDelay, string IPAddress)> instrumentInfo = new ();
        instrumentInfo.Add( "2450", (2, 0, "192.168.0.152") );
        instrumentInfo.Add( "2600", (2, 0, "192.168.0.50") );
        instrumentInfo.Add( "6510", (2, 1, "192.168.0.154") );
        instrumentInfo.Add( "7510", (2, 1, "192.168.0.144") );

        string instrument = "2600";
        TimeSpan readAfterWriteDelay = TimeSpan.FromMilliseconds( instrumentInfo[instrument].ReadAfterWriteDelay );
        int interqueryDelayMs = instrumentInfo[instrument].InterQqueryDelay;
        string ipAddress = instrumentInfo[instrument].IPAddress;

        this.InstrumentLabel.Text = $"{instrument} Delays: Read: {readAfterWriteDelay.TotalMilliseconds:0}ms; Write: {interqueryDelayMs}ms";

        System.Text.StringBuilder builder = new();
        var session = new Session( ipAddress, int.Parse( portNumber ) );
        session.ReadAfterWriteDelay = readAfterWriteDelay;

        string response = QueryDevice( session, command );
        _ = builder.Append( $"{this._count}.a: {(string.IsNullOrEmpty( response ) ? "\n" : response)}" );

        if ( interqueryDelayMs > 0 ) System.Threading.Thread.Sleep( interqueryDelayMs );
        session = new Session( ipAddress, int.Parse( portNumber ) );
        response = QueryDevice( session, command );
        _ = builder.Append( $"{this._count}.b: {(string.IsNullOrEmpty( response ) ? "\n" : response)}" );

        if ( interqueryDelayMs > 0 ) System.Threading.Thread.Sleep( interqueryDelayMs );
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

