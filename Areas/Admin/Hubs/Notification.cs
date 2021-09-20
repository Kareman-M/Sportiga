using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sportiga.Areas.Admin.Hubs
{
    public class Notification : Hub
    {
        public async Task SendNotification ()
        {
            await Clients.Group("Publish").SendAsync("ReseiveNotification");
        }
        public override async Task OnConnectedAsync()
        {
            // 1. Add the use to the public group

            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, "PublicGroup");

            // 2. Add user to the private channel, single person

            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, this.Context.User.Identity.Name);

            if (this.Context.User.IsInRole("Admin") || this.Context.User.IsInRole("Desk"))
            {
                // 3. Add the user to the Admin group

                await this.Groups.AddToGroupAsync("Publish", this.Context.ConnectionId);
            }

            // add to other groups...

            await base.OnConnectedAsync();
        }
    }
}
