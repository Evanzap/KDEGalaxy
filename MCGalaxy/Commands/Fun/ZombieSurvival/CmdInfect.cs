﻿/*
    Copyright 2011 MCForge
    
    Dual-licensed under the Educational Community License, Version 2.0 and
    the GNU General Public License, Version 3 (the "Licenses"); you may
    not use this file except in compliance with the Licenses. You may
    obtain a copy of the Licenses at
    
    http://www.opensource.org/licenses/ecl2.php
    http://www.gnu.org/licenses/gpl-3.0.html
    
    Unless required by applicable law or agreed to in writing,
    software distributed under the Licenses are distributed on an "AS IS"
    BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
    or implied. See the Licenses for the specific language governing
    permissions and limitations under the Licenses.
*/
using MCGalaxy.Games;

namespace MCGalaxy.Commands.Fun {    
    public sealed class CmdInfect : Command2 {        
        public override string name { get { return "Infect"; } }
        public override string type { get { return CommandTypes.Games; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public override CommandEnable Enabled { get { return CommandEnable.Zombie; } }      
        
        public override void Use(Player p, string message, CommandData data) {
            Player who = message.Length == 0 ? p : PlayerInfo.FindMatches(p, message);
            if (who == null) return;
            
            if (!ZSGame.Instance.RoundInProgress || ZSGame.IsInfected(who)) {
                p.Message("Cannot infect player");
            } else if (!who.Game.Referee) {
                ZSGame.Instance.InfectPlayer(who, p);
                Chat.MessageFrom(who, "λNICK &Swas infected.");
            }
        }
        
        public override void Help(Player p) {
            p.Message("&T/Infect [name]");
            p.Message("&HTurns [name] into a zombie");
        }
    }
}
