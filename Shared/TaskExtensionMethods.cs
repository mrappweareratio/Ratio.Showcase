using System;
using System.Threading.Tasks;

namespace Ratio.Showcase.Shared
{
    public static class TaskExtensionMethods
    {
        /// <summary>
        /// Executes a cancellable task or async method.
        /// Swallows all exceptions and raises a cancelled delegate or a faulted delegate with the erroring Exception passed through
        /// http://msdn.microsoft.com/en-us/library/dd997415.aspx        
        /// </summary>
        async public static Task<bool> Try(this Task task, Action<Exception> taskFaultedHandler = null, Action taskCancelledHandler = null)
        {
            var succeeded = false;
            try
            {
                await task;
                succeeded = true;
            }
            catch (AggregateException ae)
            {
                ae.Flatten().Handle((ex) =>
                {
                    if (ex is OperationCanceledException)
                    {
                        if (taskCancelledHandler != null)
                        {
                            taskCancelledHandler();
                        }
                    }
                    else
                    {
                        if (taskFaultedHandler != null)
                        {
                            taskFaultedHandler(ex);
                        }
                    }
                    return true;
                });
            }
            catch (OperationCanceledException)
            {
                if (taskCancelledHandler != null)
                {
                    taskCancelledHandler();
                }
            }
            catch (Exception ex)
            {
                if (taskFaultedHandler != null)
                {
                    taskFaultedHandler(ex);
                }
            }
            return succeeded;
        }

        /// <summary>
        /// Executes a cancellable task or async method.
        /// Swallows all exceptions and raises a cancelled delegate or a faulted delegate with the erroring Exception passed through
        /// http://msdn.microsoft.com/en-us/library/dd997415.aspx        
        /// </summary>
        async public static Task<TResult> Try<TResult>(this Task<TResult> task, Action<Exception> taskFaultedHandler = null, Action taskCancelledHandler = null)
        {
            var result = default(TResult);
            try
            {
                result = await task;
            }
            catch (AggregateException ae)
            {
                ae.Flatten().Handle((ex) =>
                {
                    if (ex is OperationCanceledException)
                    {
                        if (taskCancelledHandler != null)
                        {
                            taskCancelledHandler();
                        }
                    }
                    else
                    {
                        if (taskFaultedHandler != null)
                        {
                            taskFaultedHandler(ex);
                        }
                    }
                    return true;
                });
            }
            catch (OperationCanceledException)
            {
                if (taskCancelledHandler != null)
                {
                    taskCancelledHandler();
                }
            }
            catch (Exception ex)
            {
                if (taskFaultedHandler != null)
                {
                    taskFaultedHandler(ex);
                }
            }
            return result;
        }

        /// <summary>
        /// Executes a cancellable task or async method.
        /// Swallows all exceptions and raises a cancelled delegate or a faulted delegate with the erroring Exception passed through.
        /// Waits on both the faulted and Cancelled handlers
        /// http://msdn.microsoft.com/en-us/library/dd997415.aspx        
        /// </summary>
        async public static Task<TResult> TryCatchAsync<TResult>(this Task<TResult> task, Func<Exception, Task> taskFaultedHandler = null, Func<Task> taskCancelledHandler = null)
        {
            OperationCanceledException operationCanceledException = null;
            Exception faultedException = null;

            var result = default(TResult);
            try
            {
                result = await task.ConfigureAwait(false);
            }
            catch (AggregateException ae)
            {
                ae.Flatten().Handle((ex) =>
                {
                    var canceledException = ex as OperationCanceledException;
                    if (canceledException != null)
                    {
                        operationCanceledException = canceledException;
                    }
                    else
                    {
                        faultedException = ex;
                    }
                    return true;
                });
            }
            catch (OperationCanceledException oc)
            {
                operationCanceledException = oc;
            }
            catch (Exception ex)
            {
                faultedException = ex;
            }

            if (operationCanceledException != null)
            {
                if (taskCancelledHandler != null)
                {
                    await taskCancelledHandler().ConfigureAwait(false);
                }
            }

            if (faultedException != null)
            {
                if (taskFaultedHandler != null)
                {
                    await taskFaultedHandler(faultedException);
                }
            }

            return result;
        }
    }
}
