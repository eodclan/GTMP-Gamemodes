﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using System.Timers;
using FactionLife.Server.Services.VehicleService;

namespace FactionLife.Server.Services.FactionService
{
    class TrashJob : CheckpointJob
    {
        public int jobStage { get; private set; }
        private const int jobPointCount = 4;
        private Random rdm = new Random();
        private Boolean cooldown = false;
        private Vehicle workVehicle = null;
        private Timer trashLoadTimer = new Timer();
        private Timer finishJobTimer = new Timer();
        private Timer exitVehicleTimer = new Timer();
        private Timer exitJobCooldownTimer = new Timer();
        private ClientCheckpoint currentCheckPoint = null;

        // Fix Z
        /// <summary>
        /// List of trash pickup points
        /// 4 are picked randomly
        /// </summary>
        private Vector3[] positions =
        {
            new Vector3(-162.9298, -1668.545, 31.64808),
            new Vector3(-189.5532, -1375.702, 29.77444),
            new Vector3(-560.0544, -707.5613, 31.50684),
            new Vector3(-1159.908, -1457.231, 2.861255),
            new Vector3(599.4404, 147.7233, 96.55682),
            new Vector3(-1185.603, -1088.889, 0.813894),
            new Vector3(-1236.442, -1404.753, 2.790302),
            new Vector3(-708.5078, -725.743, 27.21856),
            new Vector3(-1500.967, -887.9347, 8.626642),
            new Vector3(1079.971, -1968.367, 29.556),
            new Vector3(-468.9455, -1729.26, 17.16786),
            new Vector3(-705.4124, -2537.478, 12.51044),
            new Vector3(-1232.402, -692.3838, 22.14487),
            new Vector3(-1296.669, -619.4992, 25.61686),
            new Vector3(-1520.694, -411.7015, 34.027),
            new Vector3(-553.3751, 307.9679, 81.742),
            new Vector3(-277.0289, 204.4182, 84.25507)
        };

        /// <summary>
        /// List of job completion points
        /// One is picked at random
        /// </summary>
        private Vector3[] endPoints =
        {
            new Vector3(453.981, -1965.712, 21.97292),
            new Vector3(-53.35933, -1317.924, 27.98088),
            new Vector3(-451.8873, -1696.857, 21.967725)
        };

        public TrashJob(CharacterHandler c) : base(c)
        {
            trashLoadTimer.Elapsed += FinishedLoadingTrash;
            finishJobTimer.Elapsed += FinishUnloadingTrash;
            exitVehicleTimer.Elapsed += ExitVehicleTimerExpire;
            exitJobCooldownTimer.Elapsed += EnableJob;

            exitVehicleTimer.Interval = 15000;
            exitVehicleTimer.AutoReset = false;

            exitJobCooldownTimer.Interval = 600000;
            exitVehicleTimer.AutoReset = false;
        }


        private void EnableJob(System.Object source, ElapsedEventArgs e)
        {
            cooldown = true;
        }

        /// <summary>
        /// Ends job if player is too long out of the work vehicle
        /// </summary>
        /// <param name="source">Timer</param>
        /// <param name="e">Timer arguments</param>
        private void ExitVehicleTimerExpire(System.Object source, ElapsedEventArgs e)
        {
            EndJob();
        }

        /// <summary>
        /// Ran when player exits vehicle
        /// Starts timer to enter vehicle again
        /// </summary>
        /// <param name="c">Client who exited vehicle</param>
        /// <param name="vHandle">Vehicle handle</param>
        private void PlayerExitedVehicle(Client c, NetHandle vHandle, int seat)
        {
            if (c.handle == character.owner.client.handle && vHandle == workVehicle.handle)
            {
                API.shared.sendNotificationToPlayer(character.owner.client, "Sie haben 15 Sekunden, um wieder ins Arbeitsfahrzeug zu kommen!");
                exitVehicleTimer.Start();
            }
        }

        /// <summary>
        /// Ran when player enters vehicle
        /// Stops the timer to enter vehicle
        /// </summary>
        /// <param name="c">Client who entered vehicle</param>
        /// <param name="vHandle">Vehicle handle</param>
        private void PlayerEnteredVehicle(Client c, NetHandle vHandle, int seat)
        {
            if (c.handle == character.owner.client.handle && vHandle == workVehicle.handle)
                exitVehicleTimer.Stop();
        }

        /// <summary>
        /// Cleans up the job when it ends
        /// </summary>
        private void CleanUp()
        {
            VehicleManager.Instance().UnsubscribeFromVehicleEnterEvent(this.PlayerEnteredVehicle);
            VehicleManager.Instance().UnsubscribeFromVehicleDestroyedEvent(this.JobVehicleDestroyed);
            VehicleManager.Instance().UnsubscribeFromVehicleExitEvent(this.PlayerExitedVehicle);
            this.RemoveAllCheckpoints();
            this.isActive = false;
            currentCheckPoint = null;
            workVehicle = null;
            jobStage = 0;
        }

        /// <summary>
        /// Ran when the work vehicle is destroyed
        /// </summary>
        /// <param name="vHandle">Vehicle handle</param>
        private void JobVehicleDestroyed(NetHandle vHandle)
        {
            if (workVehicle.handle == vHandle)
            {
                API.shared.sendNotificationToPlayer(character.owner.client, "Fahrzeug zerstört! Aufgabe fehlgeschlagen!");
                EndJob();
            }
        }

        /// <summary>
        /// Checks if player is in work vehicle
        /// </summary>
        /// <returns>True if player is in work vehicle, false otherwise</returns>
        private Boolean IsPlayerInWorkVehicle()
        {
            return this.workVehicle.occupants.Contains(character.owner.client);
        }

        /// <summary>
        /// Checks if player is in trash master vehicle
        /// </summary>
        /// <returns>True if player is in trash master, false otherwise</returns>
        private Boolean IsPlayerInTrashMaster()
        {
            if (this.IsPlayerInVehicle())
            {
                if (this.GetPlayerVehicleHash().Equals(API.shared.vehicleNameToModel("Trash")) || this.GetPlayerVehicleHash().Equals(API.shared.vehicleNameToModel("Trash2")))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Spawns the trash collection points randomly
        /// </summary>
        private void RandomizeTrashPoints()
        {
            List<int> usedNumbers = new List<int>();

            for (int i = 0; i < jobPointCount; i++)
            {
                int number = rdm.Next(0, positions.Count());

                while (usedNumbers.Contains(number))
                    number = rdm.Next(0, positions.Count());

                this.AddCheckpoint(positions.ElementAt(number), 1);
                usedNumbers.Add(number);
            }
        }

        /// <summary>
        /// Checks if missions is complete and moves to next stage
        /// </summary>
        private void CheckMissionComplete()
        {
            if (GetCheckpointCount() == 0)
            {
                this.jobStage = 1;
                API.shared.sendNotificationToPlayer(character.owner.client, "Aufgabe abgeschlossen! Zurück zum markierten Lieferpunkt");
                this.AddCheckpoint(endPoints.ElementAt(rdm.Next(0, endPoints.Count() - 1)), 1);
            }
        }

        /// <summary>
        /// Loads trash from a trash collection point
        /// </summary>
        /// <param name="cp">Trash collection point</param>
        private void LoadTrash(ClientCheckpoint cp)
        {
            workVehicle.freezePosition = true;
            trashLoadTimer.Interval = 5000;
            trashLoadTimer.Enabled = true;
            currentCheckPoint = cp;
            API.shared.sendNotificationToPlayer(character.owner.client, "Müll wird geladen ...", true);
            //API.shared.triggerClientEvent(character.client, "EVENT_CREATE_TIMER_BAR", "Test", 5.0f);
        }

        /// <summary>
        /// Unloads trash at selected point
        /// </summary>
        private void UnloadTrash()
        {
            workVehicle.freezePosition = true;
            finishJobTimer.Interval = 5000;
            finishJobTimer.Enabled = true;
            API.shared.sendNotificationToPlayer(character.owner.client, "Müll wird entladen ...", true);
        }

        /// <summary>
        /// Is ran when trash unloading is finished
        /// </summary>
        /// <param name="source">Timer</param>
        /// <param name="args">Timer arguments</param>
        private void FinishUnloadingTrash(System.Object source, ElapsedEventArgs args)
        {
            Timer t = (Timer)source;
            t.Enabled = false;
            workVehicle.freezePosition = false;
            API.shared.playSoundFrontEnd(character.client, "FLIGHT_SCHOOL_LESSON_PASSED", "HUD_AWARDS");
            FinishJob();
        }

        // Public methods

        /// <summary>
        /// Begins the job
        /// </summary>
        public override void StartJob()
        {
            if (IsPlayerInTrashMaster())
            {
                if (!cooldown)
                {
                    this.jobStage = 0;
                    RandomizeTrashPoints();
                    workVehicle = character.owner.client.vehicle;
                    this.isActive = true;
                    VehicleManager.Instance().SubscribeToVehicleExitEvent(this.PlayerExitedVehicle);
                    VehicleManager.Instance().SubscribeToVehicleDestroyedEvent(this.JobVehicleDestroyed);
                    VehicleManager.Instance().SubscribeToVehicleEnterEvent(this.PlayerEnteredVehicle);
                }
                else
                {
                    API.shared.sendNotificationToPlayer(character.owner.client, "Du hast kürzlich deinen Job gestoppt und musst warten, bevor du wieder anfängst");
                }
            }
            else
            {
                API.shared.sendNotificationToPlayer(character.owner.client, "Sie müssen in einem Müllwagen sein, um den Job zu beginnen!");
            }
        }

        /// <summary>
        /// Ends the job (fail)
        /// </summary>
        public override void EndJob()
        {
            cooldown = true;
            exitJobCooldownTimer.Start();
            CleanUp();
        }

        /// <summary>
        /// Finishes the job (success)
        /// </summary>
        public override void FinishJob()
        {
            API.shared.sendNotificationToPlayer(character.owner.client, "Job abgeschlossen! Du hast $ 9500 verdient!");
            character.SetMoney(character.money + 9500);
            CleanUp();
        }

        /// <summary>
        /// Is ran when trash loading is finished
        /// </summary>
        /// <param name="source">Timer</param>
        /// <param name="args">Timer arguments</param>
        public void FinishedLoadingTrash(System.Object source, ElapsedEventArgs args)
        {
            Timer t = (Timer)source;
            t.Enabled = false;
            API.shared.sendNotificationToPlayer(character.owner.client, "Trash loaded", true);
            this.RemoveCheckpoint(currentCheckPoint);
            currentCheckPoint = null;

            workVehicle.freezePosition = false;
            
            API.shared.playSoundFrontEnd(character.client, "SELECT", "HUD_LIQUOR_STORE_SOUNDSET");

            CheckMissionComplete();
        }

        /// <summary>
        /// Is ran when player enters checkpoint
        /// </summary>
        /// <param name="cp">Checkpoint entered</param>
        /// <param name="e">Handle of entered thing, for example vehicle or player</param>
        public override void OnEnterCheckpoint(ClientCheckpoint cp, NetHandle e)
        {
            if (IsPlayerInWorkVehicle())
            {
                Vehicle v = API.shared.getEntityFromHandle<Vehicle>(e);

                if (jobStage == 0)
                    LoadTrash(cp);
                else if (jobStage == 1)
                    UnloadTrash();
            }

        }

        /// <summary>
        /// Is ran when player exits checkpoint
        /// </summary>
        /// <param name="cp">Checkpoint exited</param>
        /// <param name="e">Handle of exited thing, for example vehicle or player</param>
        public override void OnExitCheckpoint(ClientCheckpoint cp, NetHandle e) { }
    }
}
