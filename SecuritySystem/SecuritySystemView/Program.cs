using SecuritySystemBusinessLogic.Attributes;
using SecuritySystemBusinessLogic.BusinessLogics;
using SecuritySystemBusinessLogic.HelperModels;
using SecuritySystemBusinessLogic.Interfaces;
using SecuritySystemDatabaseImplement.Implements;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using System.Windows.Forms;
using Unity;
using Unity.Lifetime;

namespace SecuritySystemView
{
    static class Program
    {
        public static void ConfigGrid<T>(List<T> data, DataGridView grid)
        {
            var type = typeof(T);

            var config = new List<string>();
            grid.Columns.Clear();
            grid.AllowUserToAddRows = false;
            foreach ( var prop in type.GetProperties())
            {
                // Получаем список атрибутов
                var attributes = prop.GetCustomAttributes(typeof(ColumnAttribute), true);
                if (attributes != null && attributes.Length > 0)
                {
                    foreach (var attr in attributes)
                    {
                        // Ищем нужный нам атрибут
                        if (attr is ColumnAttribute columnAttribute)
                        {
                            config.Add(prop.Name);
                            var column = new DataGridViewTextBoxColumn
                            {
                                Name = prop.Name,
                                ReadOnly = true,
                                HeaderText = columnAttribute.Title,
                                Visible = columnAttribute.Visible,
                                Width = columnAttribute.Width
                            };
                            if (columnAttribute.GridViewAutoSize != GridViewAutoSize.None)
                            {
                                column.AutoSizeMode = (DataGridViewAutoSizeColumnMode)Enum.Parse(typeof(DataGridViewAutoSizeColumnMode),
                                    columnAttribute.GridViewAutoSize.ToString());
                            }
                            grid.Columns.Add(column);
                        }
                    }
                }
            }

            // Добавляем строки
            foreach (var elem in data)
            {
                List<object> objs = new List<object>();
                foreach (var conf in config)
                {
                    var value = elem.GetType().GetProperty(conf).GetValue(elem);
                    objs.Add(value);
                }
                grid.Rows.Add(objs.ToArray());
            }
        }

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var container = BuildUnityContainer();

            MailLogic.MailConfig(new MailConfig
            {
                SmtpClientHost = ConfigurationManager.AppSettings["SmtpClientHost"],
                SmtpClientPort = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpClientPort"]),
                MailLogin = ConfigurationManager.AppSettings["MailLogin"],
                MailPassword = ConfigurationManager.AppSettings["MailPassword"],
            });

            // Создаём таймер
            var timer = new System.Threading.Timer(new TimerCallback(MailCheck), new MailCheckInfo
            {
                PopHost = ConfigurationManager.AppSettings["PopHost"],
                PopPort = Convert.ToInt32(ConfigurationManager.AppSettings["PopPort"]),
                Storage = container.Resolve<IMessageInfoStorage>(),
                ClientStorage = container.Resolve<IClientStorage>()
            }, 0, 100000);

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

            currentContainer.RegisterType<IClientStorage, ClientStorage>(new HierarchicalLifetimeManager());

            currentContainer.RegisterType<IStoreHouseStorage, StoreHouseStorage>(new HierarchicalLifetimeManager());

            currentContainer.RegisterType<IImplementerStorage, ImplementerStorage>(new HierarchicalLifetimeManager());

            currentContainer.RegisterType<IMessageInfoStorage, MessageInfoStorage>(new HierarchicalLifetimeManager());

            currentContainer.RegisterType<ComponentLogic>(new HierarchicalLifetimeManager());

            currentContainer.RegisterType<OrderLogic>(new HierarchicalLifetimeManager());

            currentContainer.RegisterType<SecureLogic>(new HierarchicalLifetimeManager());

            currentContainer.RegisterType<ReportLogic>(new HierarchicalLifetimeManager());

            currentContainer.RegisterType<ClientLogic>(new HierarchicalLifetimeManager());

            currentContainer.RegisterType<StoreHouseLogic>(new HierarchicalLifetimeManager());

            currentContainer.RegisterType<WorkModeling>(new HierarchicalLifetimeManager());

            currentContainer.RegisterType<MailLogic>(new HierarchicalLifetimeManager());

            currentContainer.RegisterType<BackUpAbstractLogic, BackUpLogic>(new HierarchicalLifetimeManager());

              currentContainer.RegisterType<ImplementerLogic>(new HierarchicalLifetimeManager());

            return currentContainer;
        }

        private static void MailCheck(object obj)
        {
            MailLogic.MailCheck((MailCheckInfo)obj);
        }
    }
}
