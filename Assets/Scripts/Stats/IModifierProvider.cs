using System.Collections.Generic;

namespace RPG.Stats
{
    interface IModifierProvider
    {
        IEnumerable<float> GetAdditiveModifier(Stat stat);
    }
}
