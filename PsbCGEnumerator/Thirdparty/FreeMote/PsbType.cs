using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeMote
{
    /// <summary>
    /// PSB Type
    /// </summary>
    public enum PsbType
    {
        /// <summary>
        /// Unknown type PSB
        /// </summary>
        PSB = 0,
        /// <summary>
        /// Images (pimg, dpak)
        /// </summary>
        Pimg = 1,
        /// <summary>
        /// Script (scn)
        /// </summary>
        /// TODO: KS decompiler?
        Scn = 2,
        /// <summary>
        /// EMT project - M2 MOtion (mmo, emtproj)
        /// </summary>
        Mmo = 3,
        /// <summary>
        /// Images with Layouts (used in PS*)
        /// </summary>
        Tachie = 4,
        /// <summary>
        /// MDF Archive Index (_info.psb.m)
        /// </summary>
        ArchiveInfo = 5,
        /// <summary>
        /// BMP Font (e.g. textfont24)
        /// </summary>
        BmpFont = 6,
        /// <summary>
        /// EMT
        /// </summary>
        Motion = 7,
        /// <summary>
        /// Sound Archive
        /// </summary>
        SoundArchive = 8,
        /// <summary>
        /// Tile Map
        /// </summary>
        Map = 9,
        /// <summary>
        /// Sprite Block
        /// </summary>
        SprBlock = 10,
        /// <summary>
        /// Sprite Data (define)
        /// </summary>
        SprData = 11,
        /// <summary>
        /// CLUT - Images with Color Look-Up Table
        /// </summary>
        ClutImg = 12,
        /// <summary>
        /// Chip
        /// </summary>
        ChipImg = 13
    }
}
