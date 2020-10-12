﻿﻿namespace Laba4
{
    public class Path
    {
        public void SetPathCreateToMss(Create creator, Mss mss)
        {
            creator.NextElements.Add(mss);
            mss.PreviousElements.Add(creator);
        }
        public void SetPathMssToMss(Mss mss1, Mss mss2)
        {
            mss1.NextElements.Add(mss2);
            mss2.PreviousElements.Add(mss1);
        }
        public void SetPathMssToDespose(Mss mss, Despose desposer)
        {
            mss.NextElements.Add(desposer);
        }
        public void SetPathCreateToDespose(Create creator, Despose desposer)
        {
            creator.NextElements.Add(desposer);
        }
        public void SetNeighbours(Mss model1, Mss model2)
        {
            model1.NeighbourElements.Add(model2);
            model2.NeighbourElements.Add(model1);
        }
    }
}