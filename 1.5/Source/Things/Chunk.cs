using System.Collections.Generic;
using System.Linq;
using Verse;
using RimWorld;
using static RimWorld.GenLabel;
using UnityEngine;
using System.IO;

namespace VFETribals
{
    public class Chunk: ThingWithComps
    {
        public string cachedLabel="";


        public override string LabelNoCount {

            get{
                if(cachedLabel == "")
                {
                    cachedLabel= Stuff.label.Replace("chunk", "") + this.def.label;
                }
                return cachedLabel;
            }
        
        }

        public override Graphic Graphic
        {
            get
            {
                ThingStyleDef styleDef = StyleDef;
                if (styleDef?.Graphic != null)
                {
                    if (styleGraphicInt == null)
                    {
                        if (styleDef.graphicData != null)
                        {
                            styleGraphicInt = styleDef.graphicData.GraphicColoredFor(this);
                        }
                        else
                        {
                            styleGraphicInt = styleDef.Graphic;
                        }
                    }
                    return styleGraphicInt;
                }
                return this.StinkyGraphic;
            }
        }

        public Graphic StinkyGraphic
        {
            get
            {
                if (graphicInt == null)
                {
                    if (def.graphicData == null)
                    {
                        return BaseContent.BadGraphic;
                    }

                    graphicInt = GraphicDatabase.Get<Graphic_Single>(def.graphicData.texPath, def.graphic.Shader, def.graphicData.drawSize, this.Stuff.graphicData.color, this.Stuff.graphicData.colorTwo, def.graphicData);
                    
                }
                return graphicInt;
            }
        }

       

        

    }
}
