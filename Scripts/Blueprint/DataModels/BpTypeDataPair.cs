using System;
using System.Collections.Generic;
using DataModels;
using Enums;

namespace Blueprint
{
    [Serializable]
    public class BpTypeDataPair //genericleştir: enum & IData, veya enum, T
    {
        public BpType Type;
        public BlueprintData Data;
    }
}