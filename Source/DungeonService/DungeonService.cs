using DungeonCloud.DungeonCommon;
using DungeonCloud.DungeonService.DepthFirstGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using TinyIoC;

namespace DungeonCloud.DungeonService
{
    public class DungeonService : IDungeonService
    {
        #region Private Fields

        const string _defaultId = "default";
        const int _defaultSize = 100;

        TinyIoCContainer _container = TinyIoCContainer.Current;

        #endregion 

        #region Constructors 

        static DungeonService()
        {
            //populate the IOC container
            TinyIoCContainer container = TinyIoCContainer.Current;

            //singleton
            container.Register<IIdGenerator, IdGenerator>().AsSingleton();
            container.Register<ISeedGenerator, SeedGenerator>().AsSingleton();
            container.Register<IDungeonDepot, DungeonDepot>().AsSingleton();
            
            //multi instance 
            container.Register<DungeonConfigEntity>().AsMultiInstance();
            container.Register<IRandomizer, Randomizer>().AsMultiInstance();
            container.Register<IDungeonGenerator, DungeonGeneratorDepthFirst>().AsMultiInstance();
        }

        #endregion 



        [WebGet(UriTemplate = "/dungeons/{id}", ResponseFormat = WebMessageFormat.Json)]
        public Dungeon GetDungeon(string id)
        {
            using (TinyIoCContainer requestContainer = _container.GetChildContainer())
            {
                Dungeon result = null;
                IDungeonDepot depot = requestContainer.Resolve<IDungeonDepot>();

                if (depot.ContainsKey(id))
                {
                    DungeonConfigEntity dungeonConfig = depot[id];
                    requestContainer.Register<DungeonConfigEntity>(dungeonConfig);

                    IDungeonGenerator generator = requestContainer.Resolve<IDungeonGenerator>();

                    result = generator.Generate();
                }
                else if (id == _defaultId)
                {
                    // return the default dungeon
                    result = new Dungeon(_defaultId, _defaultSize, _defaultSize);
                }
                else
                {
                    throw new WebFaultException<string>(
                        string.Format("No dungeon exists with id {0}", id),
                        HttpStatusCode.NotFound);
                }


                return result;
            }
        }

        [WebInvoke(UriTemplate = "/dungeons", Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        public string CreateDungeon(DungeonConfig configuration)
        {            
            //use a child container for this request which will dispose everything
            using (TinyIoCContainer requestContainer = _container.GetChildContainer())
            {
                //register the requested config for others to find it
                requestContainer.Register<DungeonConfig>(configuration);

                string id = requestContainer.Resolve<IIdGenerator>().GetNextId(configuration.Name);
                DungeonConfigEntity dungeonConfig = requestContainer.Resolve<DungeonConfigEntity>();
                IDungeonDepot depot = requestContainer.Resolve<IDungeonDepot>();

                depot.Add(id, dungeonConfig);

                return id;
            }
        }

        //[WebInvoke(UriTemplate = "/dungeons", Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        //public string CreateDungeon(string config)
        //{
        //    //string id = _container.Resolve<IIdGenerator>().GetNextId(configuration.Name);

        //    return config;
        //}

    }
}
