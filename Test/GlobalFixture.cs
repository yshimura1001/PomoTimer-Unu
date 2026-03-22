using PomoTimer.AppServices;

namespace Test
{
    public class GlobalFixture
    {
        public GlobalFixture()
        {
            DIContainer.Bulid();
        }
    }

    [CollectionDefinition("Global test collection", DisableParallelization = true)]
    public class GlobalCollectionDefinition : ICollectionFixture<GlobalFixture>
    {
        // 何も書かない。ICollectionFixture<> の“紐付け”だけを行う定義クラス
    }

}
