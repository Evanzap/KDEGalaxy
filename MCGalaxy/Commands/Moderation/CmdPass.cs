/*
    Written by Jack1312
    Copyright 2011-2012 MCForge
        
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
using System;
using MCGalaxy.Authentication;

namespace MCGalaxy.Commands.Moderation {
    public sealed class CmdPass : Command2 {
        public override string name { get { return "Pass"; } }
        public override string type { get { return CommandTypes.Moderation; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public override bool LogUsage { get { return false; } }
        public override bool UpdatesLastCmd { get { return false; } }
        public override CommandPerm[] ExtraPerms {
            get { return new[] { new CommandPerm(LevelPermission.Owner, "can reset passwords") }; }
        }
        public override CommandAlias[] Aliases {
            get { return new[] { new CommandAlias("SetPass", "set"), new CommandAlias("ResetPass", "reset") }; }
        }

        public override void Use(Player p, string message, CommandData data) {
            if (data.Rank < Server.Config.VerifyAdminsRank) {
                Formatter.MessageNeedMinPerm(p, "+ can verify or set a password", Server.Config.VerifyAdminsRank); return;
            }
            
            if (!Server.Config.verifyadmins) { p.Message("Password verification is not currently enabled."); return; }
            if (message.Length == 0) { Help(p); return; }
            
            string[] args = message.SplitSpaces(2);
            if (args.Length == 2 && args[0].CaselessEq("set")) {
                SetPassword(p, args[1]);
            } else if (args.Length == 2 && args[0].CaselessEq("reset")) {
                ResetPassword(p, args[1], data);
            } else {
                VerifyPassword(p, message);
            }
        }
        
        static void VerifyPassword(Player p, string password) {
            if (!p.Unverified) { p.Message("&WYou are already verified."); return; }
            if (p.passtries >= 3) { p.Kick("Did you really think you could keep on guessing?"); return; }
            if (password.IndexOf(' ') >= 0) { p.Message("Your password must be &Wone &Sword!"); return; }

            if (!Authenticator.Current.HasPassword(p.name)) {
                p.Message("You have not &Wset a password, &Suse &T/SetPass [Password] &Wto set one!");
                return;
            } 
            
            if (Authenticator.VerifyPassword(p, password)) return;
            
             p.passtries++;
             p.Message("&WWrong Password. &SRemember your password is &Wcase sensitive.");
             p.Message("Forgot your password? Contact &W{0} &Sto &Wreset it.", Server.Config.OwnerName);
        }
        
        static void SetPassword(Player p, string password) {
            if (p.Unverified && Authenticator.Current.HasPassword(p.name)) {
                Authenticator.Current.RequiresVerification(p, "can change your password");
                p.Message("Forgot your password? Contact &W{0} &Sto &Wreset it.", Server.Config.OwnerName);
                return;
            }
            
            if (password.IndexOf(' ') >= 0) { p.Message("&WPassword must be one word."); return; }
            Authenticator.Current.StorePassword(p.name, password);
            p.Message("Your password was &aset to: &c" + password);
        }
        
        void ResetPassword(Player p, string name, CommandData data) {
            string target = PlayerInfo.FindMatchesPreferOnline(p, name);
            if (target == null) return;
            
            if (p.Unverified) {
                Authenticator.Current.RequiresVerification(p, "can reset passwords");
                return;
            }
            if (!CheckResetPerms(p, data)) return;
            
            if (Authenticator.Current.ResetPassword(target)) {
                p.Message("Reset password for {0}", p.FormatNick(target));
            } else {
                p.Message("{0} &Sdoes not have a password.", p.FormatNick(target));
            }
        }

        bool CheckResetPerms(Player p, CommandData data) {
            // check server owner name for permissions backwards compatibility
            return Server.Config.OwnerName.CaselessEq(p.name) || CheckExtraPerm(p, data, 1);
        }
        
        public override void Help(Player p) {
            p.Message("&T/Pass reset [player] &H- Resets the password for that player");
            p.Message("&T/Pass set [password] &H- Sets your password to [password]");
            p.Message("&H Note: &WDo NOT set this as your Minecraft password!");
            p.Message("&T/Pass [password]");
            p.Message("&HIf you are an admin, use this command to verify your login.");
            p.Message("&H You must be verified to use commands, modify blocks, and chat");
        }
    }
}
