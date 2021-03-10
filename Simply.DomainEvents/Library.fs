namespace Simply.DomainEvents

open System
open System.Threading
open System.Threading.Tasks

[<Interface>]
type IEventHandler<'TEvent> =
    abstract member HandleAsync: event: 'TEvent * cancellationToken: CancellationToken -> Task

[<Interface>]
type IEventInterceptor<'TEvent> =
    abstract member ShouldIntercept: event: 'TEvent -> bool
    abstract member InterceptAsync: event: 'TEvent * cancellationToken: CancellationToken -> Task

type ErrorEventArgs(error: Exception) =
  inherit EventArgs()
  member __.Error = error

type EventHandlerErrorDelegate = delegate of sender: obj * args: ErrorEventArgs -> unit
type EventInterceptorErrorDelegate = delegate of sender: obj * args: ErrorEventArgs -> unit

[<Interface>]
type IEventBus =
    inherit IDisposable

    abstract member Raise<'TEvent> : event: 'TEvent -> unit
    abstract member Raise<'TEvent> : event: 'TEvent * cancellationToken: CancellationToken -> unit

    abstract member AddEventHandler<'TEvent> : handler: IEventHandler<'TEvent> -> unit
    abstract member RemoveEventHandler<'TEvent> : handler: IEventHandler<'TEvent> -> unit

    abstract member HandleEvent<'TEvent> : callback: Action<'TEvent> -> IDisposable
    abstract member HandleEvent<'TEvent> : callback: Func<'TEvent, CancellationToken, Task> -> IDisposable

    abstract member AddEventInterceptor<'TEvent> : handler: IEventInterceptor<'TEvent> -> unit
    abstract member RemoveEventInterceptor<'TEvent> : handler: IEventInterceptor<'TEvent> -> unit

    abstract member InterceptEvent<'TEvent> : predicate: Func<'TEvent, bool>
        * interceptor: Action<'TEvent> -> IDisposable
    abstract member InterceptEvent<'TEvent> : predicate: Func<'TEvent, bool>
        * interceptor: Func<'TEvent, CancellationToken, Task> -> IDisposable

    [<CLIEvent>]
    abstract member OnEventHandlerError: IEvent<EventHandlerErrorDelegate, ErrorEventArgs>
    [<CLIEvent>]
    abstract member OnEventInterceptorError: IEvent<EventInterceptorErrorDelegate, ErrorEventArgs>

type EventBus() =
    let eventHandlerErrorEvent = new Event<EventHandlerErrorDelegate, ErrorEventArgs>()
    let eventInterceptorErrorEvent = new Event<EventInterceptorErrorDelegate, ErrorEventArgs>()


    interface IEventBus with

        member __.Raise<'TEvent> (event: 'TEvent): unit =
            raise (System.NotImplementedException())
        member __.Raise<'TEvent> (event: 'TEvent, cancellationToken: CancellationToken): unit =
            raise (System.NotImplementedException())

        member __.AddEventHandler<'TEvent> (handler: IEventHandler<'TEvent>): unit =
            raise (System.NotImplementedException())
        member __.RemoveEventHandler<'TEvent> (handler: IEventHandler<'TEvent>): unit =
            raise (System.NotImplementedException())

        member __.HandleEvent<'TEvent> (callback: Action<'TEvent>): IDisposable =
            raise (System.NotImplementedException())
        member __.HandleEvent<'TEvent> (callback: Func<'TEvent, CancellationToken, Task>): IDisposable =
            raise (System.NotImplementedException())

        member __.AddEventInterceptor<'TEvent> (handler: IEventInterceptor<'TEvent>): unit =
            raise (System.NotImplementedException())
        member __.RemoveEventInterceptor<'TEvent> (handler: IEventInterceptor<'TEvent>): unit =
            raise (System.NotImplementedException())

        member __.InterceptEvent<'TEvent> (predicate: Func<'TEvent, bool>,
            interceptor: Action<'TEvent>): IDisposable =
            raise (System.NotImplementedException())
        member __.InterceptEvent<'TEvent> (predicate: Func<'TEvent, bool>,
            interceptor: Func<'TEvent, CancellationToken, Task>): IDisposable =
            raise (System.NotImplementedException())

        [<CLIEvent>]
        member __.OnEventHandlerError: IEvent<EventHandlerErrorDelegate, ErrorEventArgs> =
            eventHandlerErrorEvent.Publish
        [<CLIEvent>]
        member __.OnEventInterceptorError: IEvent<EventInterceptorErrorDelegate, ErrorEventArgs> =
            eventInterceptorErrorEvent.Publish

        member __.Dispose(): unit = ()