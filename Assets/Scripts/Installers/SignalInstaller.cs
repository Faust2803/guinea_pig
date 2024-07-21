using Common.Signals.StatisticSignal;
using Zenject;

namespace Installers
{
    public class SignalInstaller : Installer<SignalInstaller>
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<StatisticSignal>();
            Container.DeclareSignal<SaveStatisticSignal>();
            Container.DeclareSignal<ResetStatisticsSignal>();
        }
    }
}