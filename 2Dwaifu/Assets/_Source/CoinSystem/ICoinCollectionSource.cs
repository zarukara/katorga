using System;

namespace CoinSystem
{
    public interface ICoinCollectionSource
    {
        event Action CoinCollected;
    }
}
