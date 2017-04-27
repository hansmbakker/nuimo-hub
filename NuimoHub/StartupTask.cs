using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using NuimoHub.Core.Configuration;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace NuimoHub
{
    public sealed class StartupTask : IBackgroundTask
    {
        private BackgroundTaskDeferral _deferral;
        private int _optionsHash;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            // 
            // TODO: Insert code to perform background work
            //
            // If you start any asynchronous methods here, prevent the task
            // from closing prematurely by using BackgroundTaskDeferral as
            // described in http://aka.ms/backgroundtaskdeferral
            //

            _deferral = taskInstance.GetDeferral();
            taskInstance.Canceled += OnTaskCanceled;

            NuimoOptions options = await OptionsDownloader.GetOptions();
            while (options == null)
            {
                Debug.WriteLine("Could not get settings. Retrying.");
                await Task.Delay(5000);
                options = await OptionsDownloader.GetOptions();
            }

            _optionsHash = options.GetHashCode();
            
            var nuimoHub = new NuimoHub(options);
            nuimoHub.Start();

            while (true)
            {
                var newOptions = await OptionsDownloader.GetOptions();
                if (newOptions.GetHashCode() != _optionsHash)
                {
                    Debug.WriteLine("New settings detected. Shutting down.");
                    _deferral.Complete();
                }

                await Task.Delay(5000);
            }
        }

        private void OnTaskCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            Debug.WriteLine(reason);
            if (_deferral != null)
                _deferral.Complete();
        }

    }
}