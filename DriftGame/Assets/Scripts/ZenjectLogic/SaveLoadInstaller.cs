using UnityEngine;
using Zenject;

public class SaveLoadInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        BindSaveLoadManager();
    }

    public void BindSaveLoadManager()
    {
        Container
            .Bind<SaveLoadManager>()
            .To<MirraSaveLoadManager>()
            .AsSingle();
    }
}