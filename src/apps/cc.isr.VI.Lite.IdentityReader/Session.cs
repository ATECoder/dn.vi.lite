using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Net.NetworkInformation;
using Microsoft.Maui.Controls;
using System;
using System.Runtime.CompilerServices;

namespace cc.isr.VI.Lite.IdentityReader;

/// <summary>   A <see cref="Net.Sockets.Socket"/> session. </summary>
/// <remarks>   2022-11-04. </remarks>
public class Session : IDisposable
{
    /// <summary>   Constructor. </summary>
    /// <remarks>   2022-11-04. </remarks>
    /// <param name="ipAddress">    The IP address. </param>
    /// <param name="portNumber">   The port number. </param>
    public Session(string ipAddress, int portNumber)
    {
        this.IPAddress = ipAddress;
        this.PortNumber = portNumber;
    }


    #region " idisposable support "

    /// <summary> Gets a value indicating whether this instance is disposed. </summary>
    /// <value> <c>true</c> if this instance is disposed; otherwise, <c>false</c>. </value>
    protected bool IsDisposed { get; set; }

    /// <summary> Releases unmanaged and - optionally - managed resources. </summary>
    /// <param name="disposing"> <c>true</c> to release both managed and unmanaged resources;
    /// <c>false</c> to release only unmanaged resources. </param>
    protected virtual void Dispose( bool disposing )
    {
        if ( this.IsDisposed ) return;
        try
        {
            if ( disposing )
            {
                if ( this._client is object )
                {
                    this._client.Dispose();
                    this._client = null;
                }
            }
        }
        catch
        {
        }
        finally
        {
            this.IsDisposed = true;
        }
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
    /// resources.
    /// </summary>
    public void Dispose()
    {
        // Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        this.Dispose( true );
        GC.SuppressFinalize( this );
    }

    #endregion


    /// <summary>   Gets or sets the size of the reading buffer. </summary>
    /// <value> The size of the reading buffer. </value>
    public int ReadingBufferSize { get; set; } = 2048;

    /// <summary>   Gets or sets the read termination. </summary>
    /// <value> The read termination. </value>
    public string ReadTermination { get; set; } = "\n";

    /// <summary>   Gets or sets the write termination. </summary>
    /// <value> The write termination. </value>
    public string WriteTermination { get; set; } = "\n";

    /// <summary>   Gets or sets the read timeout. </summary>
    /// <value> The read timeout. </value>
    public TimeSpan ReadTimeout { get; set; } = TimeSpan.FromSeconds(1);

    /// <summary>   Gets or sets the write timeout. </summary>
    /// <value> The write timeout. </value>
    public TimeSpan WriteTimeout { get; set; } = TimeSpan.FromSeconds( 1 );

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

    /// <summary>   Writes a line. </summary>
    /// <remarks>   2022-11-04. </remarks>
    /// <param name="command">  The command. </param>
    /// <returns>   An int. </returns>
    public int WriteLine(string command)
    {
        return this.WriteLineAsync( command ).Result;
    }


    /// <summary>   Gets the cancellation token. </summary>
    /// <value> The cancellation token. </value>
    public CancellationToken CancellationToken { get; } = new CancellationToken();

    /// <summary>   Writes a line asynchronously. </summary>
    /// <remarks>   2022-11-04. </remarks>
    /// <param name="command">  The command. </param>
    /// <returns>   The write line. </returns>
    public async Task<int> WriteLineAsync( string command )
    {
        return string.IsNullOrWhiteSpace( command )
                ? 0
                : await this.SendAsync( command, this.CancellationToken );
    }

    /// <summary>   Reads line asynchronously. </summary>
    /// <remarks>   2022-11-04. </remarks>
    /// <returns>   The line. </returns>
    public async Task<string> ReadLineAsync()
    {
        return await this.ReceiveAsync( this.CancellationToken );
    }

    /// <summary>   Reads line asynchronously and trim end. </summary>
    /// <remarks>   2022-11-04. </remarks>
    /// <returns>   The line asynchronous trim end. </returns>
    public async Task<string> ReadLineAsyncTrimEnd()
    {
        return await this.ReceiveAsyncTrimEnd( this.CancellationToken );
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
    /// <returns>   A <see cref="string" />. </returns>
    public string Query( string command )
    {
        return this.SendReceiveAsync( command, this.CancellationToken ).Result;
    }

    /// <summary>
    /// Sends a query command and reads the response removing the <see cref="ReadTermination"/>.
    /// </summary>
    /// <remarks>   2022-11-04. </remarks>
    /// <param name="command">  The command. </param>
    /// <returns>   The trim end. </returns>
    public string QueryTrimEnd( string command )
    {
        return this.SendReceiveAsyncTrimEnd( command, this.CancellationToken ).Result;
    }

    /// <summary>
    /// Sends a query command asynchronously and reads the response asynchronously.
    /// </summary>
    /// <remarks>   2022-11-04. </remarks>
    /// <param name="command">  The command. </param>
    /// <returns>   The query. </returns>
    public async Task<string> QueryAsync( string command )
    {
        return await this.SendReceiveAsync( command, this.CancellationToken );
    }

    /// <summary>
    /// Sends a query command asynchronously and reads the response asynchronously removing the <see cref="ReadTermination"/>.
    /// </summary>
    /// <remarks>   2022-11-04. </remarks>
    /// <param name="command">  The command. </param>
    /// <returns>   The query. </returns>
    public async Task<string> QueryAsyncTrimEnd( string command )
    {
        return await this.SendReceiveAsyncTrimEnd( command, this.CancellationToken );
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
            if ( pinger is not null )
            {
                pinger.Dispose();
            }
        }

        return pingable;
    }

    #region " tcp client implementation "

    private TcpClient _client = null;

    /// <summary>   Gets or sets the <see cref="Net.Sockets.Socket"/> client. </summary>
    /// <value> The client. </value>
    public TcpClient Client
    {
        get {
            this._client  = (this._client is null || this._client.Client is null) ? new TcpClient() : this._client;
            return this._client;
        }
        set => this._client = value;
    }

    /// <summary>   Gets or sets connected client. </summary>
    /// <exception cref="ApplicationException"> Thrown when an Application error condition occurs. </exception>
    /// <value> The connected client. </value>
    public TcpClient ConnectedClient
    {
        get {
            try
            {
                if ( !this.Client.Connected )
                {
                    this.Client.Connect( this.IPEndPoint );
                }
            }
            catch ( SocketException ex )
            {
                ex.Data.Add( "IPEndPoint", this.IPEndPoint.ToString() );
                throw ex;
            }
            this.Client.NoDelay = false;
            this.Client.ReceiveTimeout = ( int ) this.ReadTimeout.TotalMilliseconds;
            this.Client.SendTimeout = ( int ) this.WriteTimeout.TotalMilliseconds;
            return this.Client;
        }
        set => this.Client = value;
    }

    /// <summary>   Query if data was received from the network and is available to be read. </summary>
    /// <remarks>   2022-11-04. </remarks>
    /// <param name="timeout">  The timeout. </param>
    /// <returns>   True if reading, false if not. </returns>
    public static bool HasReading( NetworkStream stream, TimeSpan timeout )
    {
        if ( stream is null ) throw new ArgumentNullException( nameof( stream ) );
        DateTime endTime = DateTime.Now.Add( timeout );
        while ( DateTime.Now < endTime && stream.Socket.Available == 0 )
        {
        }
        return stream.Socket.Available > 0;
    }

    /// <summary>   Query if data was received from the network and is available to be read. </summary>
    /// <remarks>   2022-11-04. </remarks>
    /// <param name="timeout">  The timeout. </param>
    /// <returns>   The has reading. </returns>
    public static async Task<bool> HasReadingAsync( NetworkStream stream, TimeSpan timeout )
    {
        var t = Task<bool>.Run( () => HasReading( stream, timeout ) );
        t.Wait();
        return await t.WaitAsync( timeout );
    }

    public async Task<int> SendAsync( string message, CancellationToken ct )
    {
        if ( string.IsNullOrEmpty( message ) ) return 0;

        using TcpClient client = this.ConnectedClient;
        using NetworkStream stream = client.GetStream();

        // read any data already in the stream.
        if ( stream.DataAvailable )
        {
            _ = await stream.ReadAsync( new byte[this.ReadingBufferSize].AsMemory( 0, this.ReadingBufferSize ), ct );
        }

        byte[] userMessage = Encoding.ASCII.GetBytes( $"{message}{this.WriteTermination}" );
        await stream.WriteAsync( userMessage, ct );
        stream.Flush();
        return userMessage.Length;
    }

    public async Task<string> ReceiveAsyncUntil( NetworkStream stream, CancellationToken ct )
    {
        if ( stream is null ) throw new ArgumentNullException( nameof( stream ) );

        StringBuilder sb = new ();
        while ( stream.Socket.Available > 0 )
        {
            var buffer = new byte[this.ReadingBufferSize];
            int bytesAvailable = await stream.ReadAsync( buffer.AsMemory( 0, this.ReadingBufferSize ), ct );
            if ( bytesAvailable > 0 ) _ = sb.Append( Encoding.ASCII.GetString( buffer, 0, bytesAvailable ) );
        }
        return sb.ToString();
    }

    public async Task<string> ReceiveAsync( CancellationToken ct )
    {
        using TcpClient client = this.ConnectedClient;
        using NetworkStream stream = client.GetStream();

        if ( !await HasReadingAsync( stream, this.ReadAfterWriteDelay ) ) return string.Empty;

        var buffer = new byte[this.ReadingBufferSize];
        int bytesAvailable = await stream.ReadAsync( buffer.AsMemory( 0, this.ReadingBufferSize ), ct );
        string msg = Encoding.ASCII.GetString( buffer, 0, bytesAvailable );
        return msg;
    }

    public async Task<string> ReceiveAsyncTrimEnd( CancellationToken ct )
    {
        using TcpClient client = this.ConnectedClient;
        using NetworkStream stream = client.GetStream();

        if ( !await HasReadingAsync( stream, this.ReadAfterWriteDelay ) ) return string.Empty;

        var buffer = new byte[this.ReadingBufferSize];
        int bytesAvailable = await stream.ReadAsync( buffer.AsMemory( 0, this.ReadingBufferSize ), ct );
        string msg = Encoding.ASCII.GetString( buffer, 0, bytesAvailable - this.ReadTermination.Length );
        return msg;
    }

    public async Task<string> SendReceiveAsync( string message, CancellationToken ct )
    {
        if ( string.IsNullOrEmpty( message ) ) return string.Empty;

        using TcpClient client = this.ConnectedClient;
        using NetworkStream stream = client.GetStream();

        // read any data already in the stream.
        if ( stream.DataAvailable )
        {
            _ = await stream.ReadAsync( new byte[this.ReadingBufferSize].AsMemory( 0, this.ReadingBufferSize ), ct );
        }

        byte[] userMessage = Encoding.ASCII.GetBytes( $"{message}{this.WriteTermination}" );
        await stream.WriteAsync( userMessage, ct );
        stream.Flush();

        if ( !await HasReadingAsync( stream, this.ReadAfterWriteDelay ) ) return string.Empty;

        var buffer = new byte[this.ReadingBufferSize];
        int bytesAvailable = await stream.ReadAsync( buffer.AsMemory( 0, this.ReadingBufferSize ), ct );
        string msg = Encoding.ASCII.GetString( buffer, 0, bytesAvailable );
        return msg;
    }

    /// <summary>   Sends a receive asynchronous trim end. </summary>
    /// <remarks>   2022-11-05. </remarks>
    /// <param name="message">  The message. </param>
    /// <param name="ct">       A token that allows processing to be cancelled. </param>
    /// <returns>   A <see cref="string" />. </returns>
    public async Task<string> SendReceiveAsyncTrimEnd( string message, CancellationToken ct )
    {
        if ( string.IsNullOrEmpty( message ) ) return string.Empty;

        using TcpClient client = this.ConnectedClient;
        using NetworkStream stream = client.GetStream();

        // read any data already in the stream.
        if ( stream.DataAvailable )
        {
            _ = await stream.ReadAsync( new byte[this.ReadingBufferSize].AsMemory( 0, this.ReadingBufferSize ), ct );
        }

        byte[] userMessage = Encoding.ASCII.GetBytes( $"{message}{this.WriteTermination}" );
        await stream.WriteAsync( userMessage, ct );
        stream.Flush();

        if ( !await HasReadingAsync( stream, this.ReadAfterWriteDelay ) ) return string.Empty;

        var buffer = new byte[this.ReadingBufferSize];
        int bytesAvailable = await stream.ReadAsync( buffer.AsMemory( 0, this.ReadingBufferSize ), ct );
        string msg = Encoding.ASCII.GetString( buffer, 0, bytesAvailable - this.ReadTermination.Length );
        return msg;
    }

    /// <summary>   Query if this object has reading. </summary>
    /// <remarks>   2022-11-04. </remarks>
    /// <param name="timeout">  The timeout. </param>
    /// <returns>   True if reading, false if not. </returns>
    public bool HasReading( TimeSpan timeout )
    {
        DateTime endTime = DateTime.Now.Add( timeout );
        while ( DateTime.Now < endTime && this.ConnectedClient.Available == 0 )
        {
        }
        return this.ConnectedClient.Available > 0;
    }

    /// <summary>   Has reading asynchronous. </summary>
    /// <remarks>   2022-11-04. </remarks>
    /// <param name="timeout">  The timeout. </param>
    /// <returns>   The has reading. </returns>
    public async Task<bool> HasReadingAsync( TimeSpan timeout )
    {
        return await Task<bool>.Run( () => this.HasReading( timeout ) );
    }

    #endregion

}
