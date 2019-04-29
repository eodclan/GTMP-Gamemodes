using System;
using System.Collections.Generic;
using GrandTheftMultiplayer.Server.Elements;

namespace FactionLife.Server.Phone {
    /// <summary>
    /// Сущность записи в телефоне игрока
    /// </summary>
    public class PhoneContact {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Идентификатор игрока
        /// </summary>
        public long CharacterId { get; set; }

        /// <summary>
        /// Имя контакта
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Телефонный номер
        /// </summary>
        public int Phone { get; set; }
    }
}