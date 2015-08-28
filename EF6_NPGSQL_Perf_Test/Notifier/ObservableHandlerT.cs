using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace EF6_NPGSQL_Perf_Test.Notifier
{
  public sealed class ObservableHandler<T> where T : class, INotifyPropertyChanged
  {
    private readonly LinkedList<Handler> _handlers = new LinkedList<Handler>();

    private readonly Dictionary<string, LinkedListNode<Handler>> _properties =
      new Dictionary<string, LinkedListNode<Handler>>();

    private readonly WeakEventListener<T> _listener;
#if DEBUG
    private readonly int createdThreadID;
#endif
    private bool _isCleanedUp;

    /// <summary>
    /// Initializes a new instance of the <see cref="ObservableHandler{T}" /> class.
    /// </summary>
    /// <param name="source">
    /// The <type name="T"/> object to monitor.
    /// </param>
    public ObservableHandler( T source )
    {
#if DEBUG
      createdThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
#endif
      if ( source == null )
        throw new ArgumentNullException( "source" );

      Source = new WeakReference<T>( source );
      _listener = new WeakEventListener<T>( this );
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ObservableHandler{T}" /> class.
    /// </summary>
    /// <param name="source">
    /// The <type name="T"/> object to monitor.
    /// </param>
    public static ObservableHandler<T> CreateInstance( T source )
    {
      return new ObservableHandler<T>( source );
    }

    /// <summary>
    /// Cleans up the handler by ensuring that registered handlers are
    /// no longer called. Calling this method is optional and only
    /// necessary if you need to ensure your handlers are no longer called
    /// as the source changes.
    /// </summary>
    public void Cleanup()
    {
      if ( _isCleanedUp || Source == null )
        return;

      _isCleanedUp = true;

      T source;
      Source.TryGetTarget( out source );
      if ( source != null )
      {
        foreach ( var property in _properties.Keys )
          PropertyChangedEventManager.RemoveListener( source, _listener, property );
      }
      Source = null;

      // Make sure to clear these collections so we don't hold references to the
      // delegates (which in turn hold references to the objects they are for).
      _properties.Clear();
      _handlers.Clear();
    }

    /// <summary>
    /// Adds an event handler for when the specified property changes.
    /// </summary>
    /// <param name="propertyName">The name of the property.</param>
    /// <param name="handler">The handler called when the property changes.</param>
    /// <remarks>
    /// Any existing handler is replaced if this function is called a second time
    /// for these same property.
    /// </remarks>
    public void Add( string propertyName, Action handler )
    {
#if DEBUG
      if ( System.Threading.Thread.CurrentThread.ManagedThreadId != createdThreadID )
        throw new InvalidOperationException( "Do not add handlers from a different thread than this instance was created on." );
#endif
      Add( propertyName, new Handler { PropertyChangedWithNoArgs = handler }, false );
    }

    /// <summary>
    /// Adds an event handler for when the specified property changes.
    /// </summary>
    /// <param name="propertyName">The name of the property.</param>
    /// <param name="handler">The handler called when the property changes.</param>
    /// <remarks>
    /// Any existing handler is replaced if this function is called a second time
    /// for these same property.
    /// </remarks>
    public void Add( string propertyName, Action<ObservableHandler<T>> handler )
    {
#if DEBUG
      if ( System.Threading.Thread.CurrentThread.ManagedThreadId != createdThreadID )
        throw new InvalidOperationException( "Do not add handlers from a different thread than this instance was created on." );
#endif
      Add( propertyName, new Handler { PropertyChangedWithSender = handler }, false );
    }

    /// <summary>
    /// Adds an event handler for when the specified property changes.
    /// </summary>
    /// <param name="propertyName">The name of the property.</param>
    /// <param name="handler">The handler called when the property changes.</param>
    /// <remarks>
    /// Any existing handler is replaced if this function is called a second time
    /// for these same property.
    /// </remarks>
    public void Add( string propertyName, Action<T> handler )
    {
#if DEBUG
      if ( System.Threading.Thread.CurrentThread.ManagedThreadId != createdThreadID )
        throw new InvalidOperationException( "Do not add handlers from a different thread than this instance was created on." );
#endif
      Add( propertyName, new Handler { PropertyChangedWithSource = handler }, false );
    }

    /// <summary>
    /// Adds an event handler for when the specified property changes and calls it
    /// once while adding it.
    /// </summary>
    /// <param name="propertyName">The name of the property.</param>
    /// <param name="handler">The handler called when the property changes.</param>
    /// <remarks>
    /// Any existing handler is replaced if this function is called a second time
    /// for these same property.
    /// </remarks>
    public void AddAndInvoke( string propertyName, Action handler )
    {
      Add( propertyName, new Handler { PropertyChangedWithNoArgs = handler }, true );
    }

    /// <summary>
    /// Adds an event handler for when the specified property changes and calls it
    /// once while adding it.
    /// </summary>
    /// <param name="propertyName">The name of the property.</param>
    /// <param name="handler">The handler called when the property changes.</param>
    /// <remarks>
    /// Any existing handler is replaced if this function is called a second time
    /// for these same property.
    /// </remarks>
    public void AddAndInvoke( string propertyName, Action<ObservableHandler<T>> handler )
    {
      Add( propertyName, new Handler { PropertyChangedWithSender = handler }, true );
    }

    /// <summary>
    /// Adds an event handler for when the specified property changes, and calls it
    /// once while adding it.
    /// </summary>
    /// <param name="propertyName">The name of the property.</param>
    /// <param name="handler">The handler called when the property changes.</param>
    /// <remarks>
    /// Any existing handler is replaced if this function is called a second time
    /// for these same property.
    /// </remarks>
    public void AddAndInvoke( string propertyName, Action<T> handler )
    {
      Add( propertyName, new Handler { PropertyChangedWithSource = handler }, true );
    }

    /// <summary>
    /// Removes the event handler for the specified property name.
    /// </summary>
    /// <param name="propertyName">The name of the property.</param>
    public void Remove( string propertyName )
    {
      if ( _isCleanedUp )
        throw new InvalidOperationException( "The handler has been cleaned up." );

      if ( string.IsNullOrEmpty( propertyName ) )
        throw new ArgumentNullException( "propertyName" );

      T source;
      Source.TryGetTarget( out source );
      if ( source == null )
        return;

      source.VerifyPropertyName( propertyName );

      LinkedListNode<Handler> node;
      if ( !_properties.TryGetValue( propertyName, out node ) )
        throw new ArgumentException( "Invalid property: " + propertyName, "propertyName" );

      _properties.Remove( propertyName );
      _handlers.Remove( node );
      PropertyChangedEventManager.RemoveListener( source, _listener, propertyName );
    }

    /// <summary>
    /// Recieves the property change event and attempts to call the event handlers.
    /// </summary>
    /// <param name="managerType">The type</param>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The property changed event arguments.</param>
    /// <returns>True ifthe event was handled, else false.</returns>
    public bool ReceiveWeakEvent( Type managerType, object sender, EventArgs e )
    {
      var propertyChangedEventArgs = e as PropertyChangedEventArgs;
      if ( propertyChangedEventArgs == null )
        return false;

      OnSourcePropertyChanged( propertyChangedEventArgs );

      return true;
    }

    private void OnSourcePropertyChanged( PropertyChangedEventArgs e )
    {
      if ( string.IsNullOrEmpty( e.PropertyName ) )
      {
        foreach ( var handler in _handlers )
          CallHandler( handler );
      }
      else
      {
        LinkedListNode<Handler> node;
        if ( _properties.TryGetValue( e.PropertyName, out node ) )
          CallHandler( node.Value );
      }
    }

    private void Add( string propertyName, Handler handler, bool invoke )
    {
      if ( _isCleanedUp )
        throw new InvalidOperationException( "The handler has been cleaned up." );

      if ( string.IsNullOrEmpty( propertyName ) )
        throw new ArgumentNullException( "propertyName" );

      T source;
      Source.TryGetTarget( out source );
      if ( source == null )
        return;

      source.VerifyPropertyName( propertyName );

      _properties[propertyName] = _handlers.AddLast( handler );
      PropertyChangedEventManager.AddListener( source, _listener, propertyName );

      if ( invoke )
        CallHandler( handler );
    }

    private void CallHandler( Handler handler )
    {
#if DEBUG
      if ( System.Threading.Thread.CurrentThread.ManagedThreadId != createdThreadID )
        throw new InvalidOperationException( "This handler should not be called from a different thread than it was created on." );
#endif
      if ( handler.PropertyChangedWithSender != null )
        handler.PropertyChangedWithSender( this );
      else if ( handler.PropertyChangedWithSource != null )
      {
        T source;
        Source.TryGetTarget( out source );
        if ( source != null )
          handler.PropertyChangedWithSource( source );
      }

      else
        handler.PropertyChangedWithNoArgs();
    }

    /// <summary>
    /// Gets the source <see cref="INotifyPropertyChanged" />.
    /// </summary>
    public WeakReference<T> Source { get; private set; }

    private struct Handler
    {
      public Action PropertyChangedWithNoArgs;
      public Action<ObservableHandler<T>> PropertyChangedWithSender;
      public Action<T> PropertyChangedWithSource;
    }

    private class WeakEventListener<TU> : IWeakEventListener
      where TU : class, INotifyPropertyChanged
    {
      private readonly ObservableHandler<TU> _handler;

      public WeakEventListener( ObservableHandler<TU> handler )
      {
        _handler = handler;
      }

      public bool ReceiveWeakEvent( Type managerType, object sender, EventArgs e )
      {
        var propertyChangedEventArgs = e as PropertyChangedEventArgs;
        if ( propertyChangedEventArgs == null )
          return false;

        _handler.OnSourcePropertyChanged( propertyChangedEventArgs );

        return true;
      }
    }
  }
}
