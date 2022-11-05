using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Net.NetworkInformation;

namespace isr.VI.Lite.IdentityReader;

/// <summary>   A <see cref="System.Net.Sockets.Socket"/> session. </summary>
/// <remarks>   2022-11-04. </remarks>
public class Session
{
    /// <summary>   Constructor. </summary>
    /// <remarks>   2022-11-04. </remarks>
    /// <param name="ipAddress">    The IP address. </param>
    /// <param name="portNumber">   The port number. </param>
    public Session(string ipAddress, int portNumber)
    {
        this.IPAddress = ipAddress;
        this.PortNumber = portNumber;
        //if (!this.ConnectedClient.Connected)
        //  throw new ApplicationException("Instrument at "+ this.IPEndPoint.Address + ":" + this.IPEndPoint.Port + " is not connected");
    }

    /// <summary>   Gets or sets the read termination. </summary>
    /// <value> The read termination. </value>
    public string ReadTermination { get; set; } = "\n";

    /// <summary>   Gets or sets the write termination. </summary>
    /// <value> The write termination. </value>
    public string WriteTermination { get; set; } = "\n";

    /// <summary>   Gets or sets the read timeout. </summary>
    /// <value> The read timeout. </value>
    public TimeSpan ReadTimeout { get; set; } = TimeSpan.FromSeconds(1);

    /// <summary>   Gets or sets the read after write delay. </summary>
    /// <value> The read after write delay. </value>
    public TimeSpan ReadAfterWriteDelay { get; set; } = TimeSpan.FromSeconds( 0.005 );

    /// <summary>   Gets or sets the IP address. </summary>
    /// <value> The IP address. </value>
    public string IPAddress { get; private set; }

    /// <summary>   Gets or sets the port number. </summary>
    /// <value> The port number. </value>
    public int PortNumber { get; private set; }

    private IPEndPoint _ipEndPoint = null;
    /// <summary>   Gets or sets the IP end point. </summary>
    /// <value> The IP end point. </value>
    public IPEndPoint IPEndPoint
    {
        get {
            this._ipEndPoint ??= new IPEndPoint( System.Net.IPAddress.Parse( this.IPAddress ), this.PortNumber );
            return this._ipEndPoint;
        }
        set => this._ipEndPoint = value;
    }

    private Socket _client = null;

    /// <summary>   Gets or sets the <see cref="System.Net.Sockets.Socket"/> client. </summary>
    /// <value> The client. </value>
    public Socket Client
    {
        get {
            this._client ??= new Socket( AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp );
            return this._client;
        }
        set => this._client = value;
    }

    /// <summary>   Gets or sets connected client. </summary>
    /// <exception cref="ApplicationException"> Thrown when an Application error condition occurs. </exception>
    /// <value> The connected client. </value>
    public Socket ConnectedClient
    {
        get {
            try
            {
                if ( !this.Client.Connected )
                    this.Client.Connect( this.IPEndPoint );
            }
            catch ( SocketException ex )
            {
                Console.WriteLine( "Unable to connect to server." );
                Console.WriteLine( $"\nException: {ex}" );
                throw new ApplicationException( "Instrument at " + this.IPEndPoint.Address + ":" + this.IPEndPoint.Port + " is not connected" );
            }
            return this.Client;
        }
        set => this.Client = value;
    }

    /// <summary>   Writes a line. </summary>
    /// <remarks>   2022-11-04. </remarks>
    /// <param name="command">  The command. </param>
    /// <returns>   An int. </returns>
    public int WriteLine(string command)
    {
        return this.WriteLineAsync( command ).Result;
    }

    /// <summary>   Writes a line asynchronously. </summary>
    /// <remarks>   2022-11-04. </remarks>
    /// <param name="command">  The command. </param>
    /// <returns>   The write line. </returns>
    public async Task<int> WriteLineAsync( string command )
    {
        return string.IsNullOrWhiteSpace( command )
                ? 0
                : await this.ConnectedClient.SendAsync( Encoding.ASCII.GetBytes( $"{command}{this.WriteTermination}" ), SocketFlags.None );
    }

    /// <summary>   Query if this object has reading. </summary>
    /// <remarks>   2022-11-04. </remarks>
    /// <returns>   True if reading, false if not. </returns>
    public bool HasReading()
    {
        DateTime endTime = DateTime.Now.Add( this.ReadTimeout );
        while ( DateTime.Now < endTime && this.ConnectedClient.Available == 0 )
        {
        }
        return this.ConnectedClient.Available > 0;
    }

    /// <summary>   Has reading asynchronous. </summary>
    /// <remarks>   2022-11-04. </remarks>
    /// <returns>   The has reading. </returns>
    public async Task<bool> HasReadingAsync()
    {
        return await Task<bool>.Run( () => this.HasReading() );
    }

    /// <summary>   Reads line asynchronously. </summary>
    /// <remarks>   2022-11-04. </remarks>
    /// <returns>   The line. </returns>
    public async Task<string> ReadLineAsync()
    {
        byte[] buffer = new byte[1024];
        Socket client = this.ConnectedClient;
        // using Socket client = this.ConnectedClient;
        int receivedDataLength = await client.ReceiveAsync( buffer, SocketFlags.None );
        // client.Shutdown( SocketShutdown.Both );
        client.Disconnect( true );
        return Encoding.ASCII.GetString( buffer, 0, receivedDataLength );
    }

    /// <summary>   Reads line asynchronously and trim end. </summary>
    /// <remarks>   2022-11-04. </remarks>
    /// <returns>   The line asynchronous trim end. </returns>
    public async Task<string> ReadLineAsyncTrimEnd()
    {
        byte[] buffer = new byte[1024];
        Socket client = this.ConnectedClient;
        // using Socket client = this.ConnectedClient;
        int receivedDataLength = await client.ReceiveAsync( buffer, SocketFlags.None );
        // client.Shutdown( SocketShutdown.Both );
        client.Disconnect( true );
        return Encoding.ASCII.GetString( buffer, 0, receivedDataLength - this.ReadTermination.Length );
    }

    /// <summary>   Reads the line. </summary>
    /// <remarks>   2022-11-04. </remarks>
    /// <returns>   The line. </returns>
    public string ReadLine()
    {
        return this.ReadLineAsync().Result;
    }

    /// <summary>   Reads line trim end. </summary>
    /// <remarks>   2022-11-04. </remarks>
    /// <returns>   The line trim end. </returns>
    public string ReadLineTrimEnd()
    {
        return this.ReadLineAsyncTrimEnd().Result;
    }

    /// <summary>   Sends a query command and reads the response. </summary>
    /// <remarks>   2022-11-04. </remarks>
    /// <param name="command">  The command. </param>
    /// <returns>   A string. </returns>
    public string Query( string command )
    {
        int sentCount = this.WriteLine( command );
        if ( sentCount > 0 )
        {
            Thread.Sleep( this.ReadAfterWriteDelay );
            return this.ReadLine();
        }
        else
            return string.Empty;
    }

    /// <summary>
    /// Sends a query command and reads the response removing the <see cref="ReadTermination"/>.
    /// </summary>
    /// <remarks>   2022-11-04. </remarks>
    /// <param name="command">  The command. </param>
    /// <returns>   The trim end. </returns>
    public string QueryTrimEnd( string command )
    {
        int sentCount = this.WriteLine( command );
        if ( sentCount > 0 )
        {
            Thread.Sleep( this.ReadAfterWriteDelay );
            return this.ReadLineTrimEnd();
        }
        else
            return string.Empty;

    }

    /// <summary>
    /// Sends a query command asynchronously and reads the response asynchronously.
    /// </summary>
    /// <remarks>   2022-11-04. </remarks>
    /// <param name="command">  The command. </param>
    /// <returns>   The query. </returns>
    public async Task<string> QueryAsync( string command )
    {
        int sentCount = await this.WriteLineAsync( command );
        if ( sentCount > 0 )
        {
            Thread.Sleep( this.ReadAfterWriteDelay );
            return await this.ReadLineAsync();
        }
        else
            return string.Empty;
    }

    /// <summary>
    /// Sends a query command asynchronously and reads the response asynchronously removing the <see cref="ReadTermination"/>.
    /// </summary>
    /// <remarks>   2022-11-04. </remarks>
    /// <param name="command">  The command. </param>
    /// <returns>   The query. </returns>
    public async Task<string> QueryAsyncTrimEnd( string command )
    {
        int sentCount = await this.WriteLineAsync( command );
        if ( sentCount > 0 )
        {
            Thread.Sleep( this.ReadAfterWriteDelay );
            return await this.ReadLineAsync();
        }
        else
            return string.Empty;
    }

    /// <summary>   Ping host. </summary>
    /// <remarks>   2022-11-04. </remarks>
    /// <param name="nameOrAddress">    The name or address. </param>
    /// <returns>   True if it succeeds, false if it fails. </returns>
    public static bool PingHost( string nameOrAddress )
    {
        bool pingable = false;
        Ping pinger = null;

        try
        {
            pinger = new Ping();
            PingReply reply = pinger.Send( nameOrAddress );
            pingable = reply.Status == IPStatus.Success;
        }
        catch ( PingException )
        {
            // Discard PingExceptions and return false;
        }
        finally
        {
            if ( pinger != null )
            {
                pinger.Dispose();
            }
        }

        return pingable;
    }
}
