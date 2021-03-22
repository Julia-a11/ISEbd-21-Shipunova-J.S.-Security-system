using SecuritySystemBusinessLogic.BusinessLogics;
using SecuritySystemBusinessLogic.Interfaces;
using SecuritySystemFileImplement.Implements;
using System;
using System.Windows.Forms;
using Unity;
using Unity.Lifetime;


namespace SecuritySystemView
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var container = BuildUnityContainer();
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(container.Resolve<FormMain>());
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var currentContainer = new UnityContainer();
           
            currentContainer.RegisterType<IComponentStorage, ComponentStorage>(new HierarchicalLifetimeManager());
          
            currentContainer.RegisterType<IOrderStorage, OrderStorage>(new HierarchicalLifetimeManager());
           
            currentContainer.RegisterType<ISecureStorage, SecureStorage>(new HierarchicalLifetimeManager());

            currentContainer.RegisterType<IStoreHouseStorage, StoreHouseStorage>(new HierarchicalLifetimeManager());

            currentContainer.RegisterType<ComponentLogic>(new HierarchicalLifetimeManager());
            
            currentContainer.RegisterType<OrderLogic>(new HierarchicalLifetimeManager());
            
            currentContainer.RegisterType<SecureLogic>(new HierarchicalLifetimeManager());

            currentContainer.RegisterType<StoreHouseLogic>(new HierarchicalLifetimeManager());

            return currentContainer;
        }
    }
}