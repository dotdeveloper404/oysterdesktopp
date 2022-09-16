//namespace FastestVPNLibrary
//{
//    using FastestVPNLibrary.Responses;
//    using System;
//    using System.Diagnostics;
//    using System.Runtime.CompilerServices;
//    using System.Threading.Tasks;

//    public class VersionDependency
//    {
//        [AsyncStateMachine(typeof(<CheckIsUpdateAvailable>d__4)), DebuggerStepThrough]
//        public static Task<checkUpdates> CheckIsUpdateAvailable(Version curVersion)
//        {
//            <CheckIsUpdateAvailable>d__4 stateMachine = new <CheckIsUpdateAvailable>d__4 {
//                curVersion = curVersion,
//                <>t__builder = AsyncTaskMethodBuilder<checkUpdates>.Create(),
//                <>1__state = -1
//            };
//            stateMachine.<>t__builder.Start<<CheckIsUpdateAvailable>d__4>(ref stateMachine);
//            return stateMachine.<>t__builder.Task;
//        }

//        public static bool IsUpdate { get; set; }

//        [CompilerGenerated]
//        private sealed class <CheckIsUpdateAvailable>d__4 : IAsyncStateMachine
//        {
//            public int <>1__state;
//            public AsyncTaskMethodBuilder<checkUpdates> <>t__builder;
//            public Version curVersion;
//            private checkUpdates <update>5__1;
//            private Version <ver>5__2;
//            private checkUpdates <>s__3;
//            private checkUpdates <chk>5__4;
//            private Exception <e>5__5;
//            private TaskAwaiter<checkUpdates> <>u__1;

//            private void MoveNext()
//            {
//                checkUpdates updates;
//                Exception exception;
//                int num = this.<>1__state;
//                try
//                {
//                    if (num != 0)
//                    {
//                        VersionDependency.IsUpdate = false;
//                    }
//                    try
//                    {
//                        TaskAwaiter<checkUpdates> awaiter;
//                        if (num != 0)
//                        {
//                            awaiter = vpn.checkUpdates().GetAwaiter();
//                            if (!awaiter.IsCompleted)
//                            {
//                                this.<>1__state = num = 0;
//                                this.<>u__1 = awaiter;
//                                VersionDependency.<CheckIsUpdateAvailable>d__4 stateMachine = this;
//                                this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<checkUpdates>, VersionDependency.<CheckIsUpdateAvailable>d__4>(ref awaiter, ref stateMachine);
//                                return;
//                            }
//                        }
//                        else
//                        {
//                            awaiter = this.<>u__1;
//                            this.<>u__1 = new TaskAwaiter<checkUpdates>();
//                            this.<>1__state = num = -1;
//                        }
//                        this.<>s__3 = awaiter.GetResult();
//                        this.<update>5__1 = this.<>s__3;
//                        this.<>s__3 = null;
//                        if (this.<update>5__1.error)
//                        {
//                            updates = new checkUpdates();
//                            goto Label_018E;
//                        }
//                        Version.TryParse((this.<update>5__1 == null) ? null : this.<update>5__1.version, out this.<ver>5__2);
//                        if (this.curVersion.CompareTo(this.<ver>5__2) < 0)
//                        {
//                            this.<chk>5__4 = new checkUpdates();
//                            this.<chk>5__4.url = this.<update>5__1.url;
//                            this.<chk>5__4.version = this.<update>5__1.version.ToString();
//                            this.<chk>5__4.isUpdateAvailable = true;
//                            updates = this.<chk>5__4;
//                            goto Label_018E;
//                        }
//                        this.<update>5__1 = null;
//                        this.<ver>5__2 = null;
//                    }
//                    catch (Exception exception1)
//                    {
//                        exception = exception1;
//                        this.<e>5__5 = exception;
//                        Console.WriteLine(this.<e>5__5.Message);
//                    }
//                    updates = new checkUpdates();
//                }
//                catch (Exception exception2)
//                {
//                    exception = exception2;
//                    this.<>1__state = -2;
//                    this.<>t__builder.SetException(exception);
//                    return;
//                }
//            Label_018E:
//                this.<>1__state = -2;
//                this.<>t__builder.SetResult(updates);
//            }

//            [DebuggerHidden]
//            private void SetStateMachine(IAsyncStateMachine stateMachine)
//            {
//            }
//        }
//    }
//}

