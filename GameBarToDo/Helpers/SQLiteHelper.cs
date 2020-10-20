using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;


namespace GameBarToDo.Helpers
{
    class SQLiteHelper
    {
        private string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Lists.db");
        private bool tablesCreated = false;

        public SQLiteHelper()
        {
            InitializeDatabase();
        }
        public void InitializeDatabase()
        {
            ApplicationData.Current.LocalFolder.CreateFileAsync("Lists.db", CreationCollisionOption.OpenIfExists);
            using (SqliteConnection db =
               new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                String createTables = @"PRAGMA foreign_keys = ON;

CREATE TABLE IF NOT EXISTS Lists (
id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
list_name TEXT NOT NULL,
created_date DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS List_items (
id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
list_ID INTEGER,
item_name TEXT NOT NULL,
is_complete BIT NOT NULL,
created_date DEFAULT CURRENT_TIMESTAMP,
FOREIGN KEY(list_ID) REFERENCES Lists(id)
);

CREATE TABLE IF NOT EXISTS Item_notes (
id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
item_ID INTEGER,
note TEXT NOT NULL,
created_date DEFAULT CURRENT_TIMESTAMP,
FOREIGN KEY(item_ID) REFERENCES List_items(id)
);";

                SqliteCommand createTable = new SqliteCommand(createTables, db);

                createTable.ExecuteReader();
                tablesCreated = true;
            }
        }

        public void LoadDummyData()
        {
            if (tablesCreated)
            {
                using (SqliteConnection db =
                   new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();
                    string insertScript = @"insert into Lists (list_name, created_date) values ('Devify', '2019-10-24 07:16:06');
insert into Lists (list_name, created_date) values ('Kamba', '2020-06-28 23:04:39');
insert into Lists (list_name, created_date) values ('Dynazzy', '2019-11-03 06:01:28');
insert into Lists (list_name, created_date) values ('Voomm', '2020-04-07 11:01:44');
insert into Lists (list_name, created_date) values ('Kwinu', '2020-06-21 15:27:48');
insert into Lists (list_name, created_date) values ('Dynazzy', '2020-03-25 20:36:14');
insert into Lists (list_name, created_date) values ('Izio', '2019-11-23 07:10:58');
insert into Lists (list_name, created_date) values ('Oyonder', '2020-02-26 14:01:07');
insert into Lists (list_name, created_date) values ('Twimm', '2020-06-28 03:57:56');
insert into Lists (list_name, created_date) values ('Dynabox', '2019-11-02 21:49:57');

insert into List_items (list_id, item_name, is_complete, created_date) values (4, 'neural-net', true, '2020-03-07 20:18:44');
insert into List_items (list_id, item_name, is_complete, created_date) values (9, 'empowering', true, '2020-08-12 22:10:11');
insert into List_items (list_id, item_name, is_complete, created_date) values (5, 'User-friendly', false, '2020-06-02 06:54:00');
insert into List_items (list_id, item_name, is_complete, created_date) values (3, 'Multi-layered', true, '2020-10-12 22:20:27');
insert into List_items (list_id, item_name, is_complete, created_date) values (7, 'high-level', false, '2019-12-12 22:34:24');
insert into List_items (list_id, item_name, is_complete, created_date) values (2, 'application', false, '2020-09-03 03:57:08');
insert into List_items (list_id, item_name, is_complete, created_date) values (9, 'portal', true, '2020-05-31 01:43:39');
insert into List_items (list_id, item_name, is_complete, created_date) values (2, 'Monitored', false, '2020-07-01 12:15:55');
insert into List_items (list_id, item_name, is_complete, created_date) values (3, 'disintermediate', true, '2020-01-26 11:33:44');
insert into List_items (list_id, item_name, is_complete, created_date) values (8, 'impactful', false, '2019-11-29 19:14:26');
insert into List_items (list_id, item_name, is_complete, created_date) values (8, 'complexity', true, '2020-07-12 15:40:33');
insert into List_items (list_id, item_name, is_complete, created_date) values (5, 'Secured', true, '2020-01-26 05:39:43');
insert into List_items (list_id, item_name, is_complete, created_date) values (8, 'Object-based', true, '2020-05-03 00:11:58');
insert into List_items (list_id, item_name, is_complete, created_date) values (9, 'executive', false, '2020-04-13 22:39:11');
insert into List_items (list_id, item_name, is_complete, created_date) values (4, 'Object-based', false, '2020-05-10 00:06:35');
insert into List_items (list_id, item_name, is_complete, created_date) values (7, 'stable', true, '2020-04-14 16:39:20');
insert into List_items (list_id, item_name, is_complete, created_date) values (6, 'Multi-lateral', false, '2020-01-19 19:43:48');
insert into List_items (list_id, item_name, is_complete, created_date) values (2, 'fresh-thinking', true, '2019-11-15 14:42:19');
insert into List_items (list_id, item_name, is_complete, created_date) values (3, 'bandwidth-monitored', true, '2020-03-04 23:51:54');
insert into List_items (list_id, item_name, is_complete, created_date) values (8, 'fault-tolerant', false, '2020-05-29 07:10:25');
insert into List_items (list_id, item_name, is_complete, created_date) values (5, 'attitude', false, '2020-09-20 11:40:00');
insert into List_items (list_id, item_name, is_complete, created_date) values (8, 'analyzing', false, '2020-07-01 21:59:50');
insert into List_items (list_id, item_name, is_complete, created_date) values (7, 'complexity', true, '2020-07-02 21:47:55');
insert into List_items (list_id, item_name, is_complete, created_date) values (2, 'Face to face', true, '2020-06-02 06:05:51');
insert into List_items (list_id, item_name, is_complete, created_date) values (2, 'grid-enabled', false, '2020-02-09 11:01:18');
insert into List_items (list_id, item_name, is_complete, created_date) values (3, 'radical', true, '2019-12-28 06:04:40');
insert into List_items (list_id, item_name, is_complete, created_date) values (8, 'tertiary', true, '2019-12-30 09:11:37');
insert into List_items (list_id, item_name, is_complete, created_date) values (8, 'demand-driven', false, '2019-10-23 16:05:07');
insert into List_items (list_id, item_name, is_complete, created_date) values (1, 'Cloned', false, '2020-04-16 05:26:56');
insert into List_items (list_id, item_name, is_complete, created_date) values (5, 'reciprocal', false, '2019-12-02 01:52:40');
insert into List_items (list_id, item_name, is_complete, created_date) values (1, 'methodology', true, '2020-08-23 15:03:09');
insert into List_items (list_id, item_name, is_complete, created_date) values (4, 'emulation', false, '2019-11-13 17:54:53');
insert into List_items (list_id, item_name, is_complete, created_date) values (5, 'Operative', true, '2020-04-14 01:21:55');
insert into List_items (list_id, item_name, is_complete, created_date) values (2, 'success', true, '2020-06-28 00:06:37');
insert into List_items (list_id, item_name, is_complete, created_date) values (8, 'groupware', false, '2020-04-18 00:55:27');
insert into List_items (list_id, item_name, is_complete, created_date) values (1, 'demand-driven', false, '2020-07-12 04:34:16');
insert into List_items (list_id, item_name, is_complete, created_date) values (6, 'Multi-layered', true, '2020-03-08 01:20:59');
insert into List_items (list_id, item_name, is_complete, created_date) values (10, 'mission-critical', false, '2020-08-28 05:13:47');
insert into List_items (list_id, item_name, is_complete, created_date) values (5, 'Automated', false, '2020-04-24 00:57:05');
insert into List_items (list_id, item_name, is_complete, created_date) values (8, 'fresh-thinking', false, '2020-05-26 15:02:41');
insert into List_items (list_id, item_name, is_complete, created_date) values (8, 'Mandatory', true, '2020-09-26 09:44:40');
insert into List_items (list_id, item_name, is_complete, created_date) values (5, 'Reduced', false, '2020-09-09 08:30:20');
insert into List_items (list_id, item_name, is_complete, created_date) values (1, 'transitional', false, '2020-03-19 05:51:41');
insert into List_items (list_id, item_name, is_complete, created_date) values (2, 'radical', true, '2020-08-29 10:24:37');
insert into List_items (list_id, item_name, is_complete, created_date) values (6, 'Phased', true, '2020-08-14 10:58:44');
insert into List_items (list_id, item_name, is_complete, created_date) values (7, 'framework', true, '2020-05-27 06:37:08');
insert into List_items (list_id, item_name, is_complete, created_date) values (3, 'success', true, '2020-10-02 18:15:47');
insert into List_items (list_id, item_name, is_complete, created_date) values (4, 'Programmable', false, '2019-11-08 15:22:58');
insert into List_items (list_id, item_name, is_complete, created_date) values (9, 'Versatile', true, '2020-07-25 15:24:15');
insert into List_items (list_id, item_name, is_complete, created_date) values (5, 'synergy', false, '2020-09-30 13:53:45');
insert into List_items (list_id, item_name, is_complete, created_date) values (5, 'standardization', false, '2020-03-26 07:03:55');
insert into List_items (list_id, item_name, is_complete, created_date) values (4, 'mobile', true, '2020-03-30 19:11:49');
insert into List_items (list_id, item_name, is_complete, created_date) values (9, 'discrete', true, '2020-02-23 17:03:56');
insert into List_items (list_id, item_name, is_complete, created_date) values (1, 'empowering', true, '2020-02-22 18:49:48');
insert into List_items (list_id, item_name, is_complete, created_date) values (2, 'intranet', false, '2019-10-24 12:46:25');
insert into List_items (list_id, item_name, is_complete, created_date) values (9, 'system-worthy', false, '2020-04-19 09:34:24');
insert into List_items (list_id, item_name, is_complete, created_date) values (4, 'matrices', true, '2020-04-12 06:47:41');
insert into List_items (list_id, item_name, is_complete, created_date) values (3, 'Cross-platform', true, '2019-11-27 05:07:08');
insert into List_items (list_id, item_name, is_complete, created_date) values (10, 'bandwidth-monitored', false, '2020-10-12 20:51:29');
insert into List_items (list_id, item_name, is_complete, created_date) values (7, 'transitional', false, '2020-09-25 10:12:41');
insert into List_items (list_id, item_name, is_complete, created_date) values (10, 'didactic', false, '2020-01-11 12:14:34');
insert into List_items (list_id, item_name, is_complete, created_date) values (9, 'conglomeration', false, '2019-11-12 19:14:29');
insert into List_items (list_id, item_name, is_complete, created_date) values (1, 'forecast', true, '2020-08-04 07:02:00');
insert into List_items (list_id, item_name, is_complete, created_date) values (1, 'structure', false, '2020-09-15 09:24:34');
insert into List_items (list_id, item_name, is_complete, created_date) values (9, 'leverage', false, '2020-01-04 03:43:31');
insert into List_items (list_id, item_name, is_complete, created_date) values (10, 'Future-proofed', true, '2020-03-29 14:18:18');
insert into List_items (list_id, item_name, is_complete, created_date) values (2, 'systematic', true, '2020-05-11 02:38:53');
insert into List_items (list_id, item_name, is_complete, created_date) values (9, 'Object-based', false, '2020-03-05 06:32:08');
insert into List_items (list_id, item_name, is_complete, created_date) values (7, 'protocol', true, '2020-09-28 18:30:56');
insert into List_items (list_id, item_name, is_complete, created_date) values (3, 'executive', false, '2020-05-17 10:53:53');
insert into List_items (list_id, item_name, is_complete, created_date) values (8, 'utilisation', false, '2020-06-05 10:11:59');
insert into List_items (list_id, item_name, is_complete, created_date) values (3, 'Cloned', false, '2019-12-10 21:13:23');
insert into List_items (list_id, item_name, is_complete, created_date) values (8, 'Proactive', true, '2020-05-27 12:02:44');
insert into List_items (list_id, item_name, is_complete, created_date) values (6, 'Extended', true, '2019-12-25 07:36:45');
insert into List_items (list_id, item_name, is_complete, created_date) values (1, 'radical', true, '2020-05-09 18:37:06');
insert into List_items (list_id, item_name, is_complete, created_date) values (10, 'systematic', false, '2019-12-04 12:35:23');
insert into List_items (list_id, item_name, is_complete, created_date) values (5, 'grid-enabled', false, '2020-03-09 18:40:51');
insert into List_items (list_id, item_name, is_complete, created_date) values (5, 'contingency', false, '2020-09-29 14:40:05');
insert into List_items (list_id, item_name, is_complete, created_date) values (8, 'motivating', true, '2020-09-23 22:33:43');
insert into List_items (list_id, item_name, is_complete, created_date) values (7, 'focus group', true, '2020-03-22 23:12:12');
insert into List_items (list_id, item_name, is_complete, created_date) values (9, 'projection', false, '2020-03-27 05:33:08');
insert into List_items (list_id, item_name, is_complete, created_date) values (9, 'Public-key', false, '2020-08-09 05:36:28');
insert into List_items (list_id, item_name, is_complete, created_date) values (5, 'portal', true, '2019-10-21 00:07:09');
insert into List_items (list_id, item_name, is_complete, created_date) values (4, 'Cross-group', true, '2020-04-18 13:09:10');
insert into List_items (list_id, item_name, is_complete, created_date) values (5, 'Face to face', true, '2020-10-01 09:45:44');
insert into List_items (list_id, item_name, is_complete, created_date) values (1, 'extranet', false, '2020-04-27 21:36:51');
insert into List_items (list_id, item_name, is_complete, created_date) values (7, 'fault-tolerant', true, '2020-02-24 11:03:15');
insert into List_items (list_id, item_name, is_complete, created_date) values (3, 'Synchronised', false, '2020-07-04 11:51:47');
insert into List_items (list_id, item_name, is_complete, created_date) values (1, 'Cloned', true, '2020-02-03 21:17:40');
insert into List_items (list_id, item_name, is_complete, created_date) values (3, 'didactic', false, '2019-12-27 12:42:10');
insert into List_items (list_id, item_name, is_complete, created_date) values (10, 'model', true, '2020-09-21 14:02:14');
insert into List_items (list_id, item_name, is_complete, created_date) values (4, 'Pre-emptive', false, '2020-05-03 00:50:03');
insert into List_items (list_id, item_name, is_complete, created_date) values (2, 'tangible', true, '2019-10-18 22:19:23');
insert into List_items (list_id, item_name, is_complete, created_date) values (4, 'Open-architected', true, '2020-02-09 03:56:50');
insert into List_items (list_id, item_name, is_complete, created_date) values (3, 'user-facing', false, '2020-03-16 21:27:08');
insert into List_items (list_id, item_name, is_complete, created_date) values (8, 'knowledge user', true, '2020-06-09 09:51:19');
insert into List_items (list_id, item_name, is_complete, created_date) values (4, 'Secured', true, '2020-08-10 22:20:28');
insert into List_items (list_id, item_name, is_complete, created_date) values (9, 'Implemented', true, '2020-03-17 00:19:19');
insert into List_items (list_id, item_name, is_complete, created_date) values (8, 'Team-oriented', true, '2020-05-19 07:27:32');
insert into List_items (list_id, item_name, is_complete, created_date) values (9, 'synergy', false, '2020-06-12 20:28:58');

insert into Item_notes (item_ID, note, created_date) values (51, 'Visionary real-time frame', '2020-02-03 17:28:11');
insert into Item_notes (item_ID, note, created_date) values (62, 'Versatile 3rd generation leverage', '2020-01-25 09:09:54');
insert into Item_notes (item_ID, note, created_date) values (88, 'Switchable solution-oriented time-frame', '2019-12-04 22:16:40');
insert into Item_notes (item_ID, note, created_date) values (5, 'Cross-platform bandwidth-monitored secured line', '2020-05-24 16:46:38');
insert into Item_notes (item_ID, note, created_date) values (100, 'Ameliorated optimizing challenge', '2020-07-23 13:30:23');
insert into Item_notes (item_ID, note, created_date) values (6, 'Compatible dedicated adapter', '2020-02-22 11:09:59');
insert into Item_notes (item_ID, note, created_date) values (83, 'Operative multi-state software', '2020-01-26 01:39:37');
insert into Item_notes (item_ID, note, created_date) values (17, 'Fundamental heuristic support', '2019-10-25 09:50:38');
insert into Item_notes (item_ID, note, created_date) values (86, 'Visionary full-range orchestration', '2019-11-03 06:03:20');
insert into Item_notes (item_ID, note, created_date) values (75, 'Multi-layered multimedia model', '2020-04-24 11:45:34');
insert into Item_notes (item_ID, note, created_date) values (33, 'Function-based impactful concept', '2019-12-18 16:36:26');
insert into Item_notes (item_ID, note, created_date) values (57, 'Integrated web-enabled ability', '2020-03-19 09:06:25');
insert into Item_notes (item_ID, note, created_date) values (85, 'Versatile background interface', '2020-09-18 23:10:18');
insert into Item_notes (item_ID, note, created_date) values (100, 'Distributed contextually-based structure', '2020-10-10 07:11:53');
insert into Item_notes (item_ID, note, created_date) values (20, 'Cross-group 24 hour software', '2020-06-13 10:25:55');
insert into Item_notes (item_ID, note, created_date) values (69, 'Total logistical protocol', '2020-09-28 02:47:58');
insert into Item_notes (item_ID, note, created_date) values (62, 'Reactive fault-tolerant encryption', '2020-01-30 07:10:14');
insert into Item_notes (item_ID, note, created_date) values (56, 'Ergonomic solution-oriented knowledge base', '2020-01-11 21:53:51');
insert into Item_notes (item_ID, note, created_date) values (32, 'Fundamental fault-tolerant internet solution', '2020-06-15 14:52:58');
insert into Item_notes (item_ID, note, created_date) values (3, 'Front-line national framework', '2019-12-21 00:12:17');
insert into Item_notes (item_ID, note, created_date) values (95, 'User-friendly needs-based alliance', '2020-05-08 14:00:28');
insert into Item_notes (item_ID, note, created_date) values (83, 'Intuitive systematic artificial intelligence', '2020-01-29 18:26:54');
insert into Item_notes (item_ID, note, created_date) values (45, 'Cross-group bifurcated ability', '2020-08-23 11:13:00');
insert into Item_notes (item_ID, note, created_date) values (74, 'Synergistic mobile help-desk', '2020-04-02 10:12:32');
insert into Item_notes (item_ID, note, created_date) values (95, 'Distributed bottom-line utilisation', '2020-09-14 16:43:25');
insert into Item_notes (item_ID, note, created_date) values (64, 'Open-source motivating access', '2019-11-20 01:39:15');
insert into Item_notes (item_ID, note, created_date) values (83, 'Open-source systemic circuit', '2020-07-09 06:55:15');
insert into Item_notes (item_ID, note, created_date) values (99, 'Front-line dedicated core', '2020-03-25 13:38:26');
insert into Item_notes (item_ID, note, created_date) values (86, 'Triple-buffered regional solution', '2020-05-03 08:25:43');
insert into Item_notes (item_ID, note, created_date) values (85, 'Diverse analyzing workforce', '2019-10-27 01:56:27');
insert into Item_notes (item_ID, note, created_date) values (77, 'Multi-channelled upward-trending concept', '2020-07-20 17:54:42');
insert into Item_notes (item_ID, note, created_date) values (6, 'Configurable heuristic standardization', '2020-04-10 12:01:10');
insert into Item_notes (item_ID, note, created_date) values (72, 'Organized mission-critical archive', '2020-08-16 20:29:11');
insert into Item_notes (item_ID, note, created_date) values (97, 'Organic hybrid matrices', '2020-04-07 22:29:13');
insert into Item_notes (item_ID, note, created_date) values (4, 'Extended multimedia access', '2019-10-21 07:22:14');
insert into Item_notes (item_ID, note, created_date) values (8, 'Team-oriented tertiary access', '2020-02-23 20:15:21');
insert into Item_notes (item_ID, note, created_date) values (94, 'Diverse maximized support', '2019-11-05 01:54:59');
insert into Item_notes (item_ID, note, created_date) values (54, 'Streamlined national interface', '2020-09-21 01:22:01');
insert into Item_notes (item_ID, note, created_date) values (70, 'Compatible dynamic implementation', '2020-07-26 11:26:18');
insert into Item_notes (item_ID, note, created_date) values (75, 'Customer-focused incremental focus group', '2020-10-04 06:31:45');
insert into Item_notes (item_ID, note, created_date) values (45, 'Re-contextualized bandwidth-monitored productivity', '2020-07-27 04:11:19');
insert into Item_notes (item_ID, note, created_date) values (93, 'Multi-channelled user-facing projection', '2019-11-11 17:59:01');
insert into Item_notes (item_ID, note, created_date) values (96, 'Re-engineered content-based attitude', '2020-02-20 00:58:56');
insert into Item_notes (item_ID, note, created_date) values (51, 'Organic optimal pricing structure', '2020-03-24 10:42:50');
insert into Item_notes (item_ID, note, created_date) values (33, 'Future-proofed system-worthy access', '2019-11-21 04:10:18');
insert into Item_notes (item_ID, note, created_date) values (18, 'Integrated incremental firmware', '2020-03-07 22:51:38');
insert into Item_notes (item_ID, note, created_date) values (18, 'Visionary systemic solution', '2020-01-20 05:40:20');
insert into Item_notes (item_ID, note, created_date) values (82, 'Enhanced upward-trending workforce', '2020-03-23 05:56:35');
insert into Item_notes (item_ID, note, created_date) values (59, 'Open-architected maximized software', '2020-08-01 10:41:36');
insert into Item_notes (item_ID, note, created_date) values (28, 'Multi-channelled responsive interface', '2020-06-06 01:19:48');
insert into Item_notes (item_ID, note, created_date) values (1, 'Monitored 4th generation workforce', '2020-04-25 10:04:31');
insert into Item_notes (item_ID, note, created_date) values (9, 'Automated didactic artificial intelligence', '2020-07-31 12:41:35');
insert into Item_notes (item_ID, note, created_date) values (53, 'Optimized analyzing neural-net', '2020-04-07 00:22:54');
insert into Item_notes (item_ID, note, created_date) values (74, 'Multi-layered background data-warehouse', '2020-10-08 04:26:46');
insert into Item_notes (item_ID, note, created_date) values (13, 'Implemented eco-centric local area network', '2020-02-22 08:51:05');
insert into Item_notes (item_ID, note, created_date) values (48, 'Compatible discrete policy', '2020-06-09 08:08:50');
insert into Item_notes (item_ID, note, created_date) values (86, 'Programmable grid-enabled focus group', '2020-09-23 04:26:32');
insert into Item_notes (item_ID, note, created_date) values (29, 'Cross-group upward-trending forecast', '2020-08-20 00:26:48');
insert into Item_notes (item_ID, note, created_date) values (11, 'Managed logistical firmware', '2020-05-24 19:19:57');
insert into Item_notes (item_ID, note, created_date) values (75, 'De-engineered optimizing emulation', '2020-10-07 10:08:19');
insert into Item_notes (item_ID, note, created_date) values (28, 'Reactive bi-directional database', '2020-04-10 20:52:43');
insert into Item_notes (item_ID, note, created_date) values (86, 'Decentralized context-sensitive definition', '2019-12-09 17:25:15');
insert into Item_notes (item_ID, note, created_date) values (70, 'Polarised next generation pricing structure', '2020-07-01 14:03:03');
insert into Item_notes (item_ID, note, created_date) values (46, 'Polarised uniform budgetary management', '2019-11-04 16:50:47');
insert into Item_notes (item_ID, note, created_date) values (60, 'Cloned 3rd generation approach', '2020-09-16 10:19:05');
insert into Item_notes (item_ID, note, created_date) values (8, 'Profit-focused static ability', '2020-02-09 01:19:00');
insert into Item_notes (item_ID, note, created_date) values (29, 'Upgradable explicit project', '2019-10-19 06:57:35');
insert into Item_notes (item_ID, note, created_date) values (43, 'Polarised value-added definition', '2020-09-10 19:25:05');
insert into Item_notes (item_ID, note, created_date) values (6, 'Streamlined web-enabled matrix', '2020-09-02 00:16:31');
insert into Item_notes (item_ID, note, created_date) values (66, 'Synergized optimal encryption', '2019-12-11 11:09:01');
insert into Item_notes (item_ID, note, created_date) values (22, 'Total scalable standardization', '2019-11-22 18:34:34');
insert into Item_notes (item_ID, note, created_date) values (60, 'Polarised mission-critical portal', '2020-02-20 10:38:40');
insert into Item_notes (item_ID, note, created_date) values (91, 'Future-proofed solution-oriented adapter', '2020-09-13 12:19:19');
insert into Item_notes (item_ID, note, created_date) values (99, 'Quality-focused global throughput', '2020-06-25 06:00:47');
insert into Item_notes (item_ID, note, created_date) values (1, 'Expanded eco-centric support', '2020-03-16 14:04:22');
insert into Item_notes (item_ID, note, created_date) values (8, 'Optional impactful flexibility', '2020-04-18 20:40:21');
insert into Item_notes (item_ID, note, created_date) values (80, 'Cloned well-modulated function', '2020-08-27 16:04:34');
insert into Item_notes (item_ID, note, created_date) values (81, 'Advanced actuating framework', '2020-01-16 08:40:42');
insert into Item_notes (item_ID, note, created_date) values (45, 'Ergonomic 24 hour model', '2020-07-20 17:14:51');
insert into Item_notes (item_ID, note, created_date) values (39, 'Intuitive secondary benchmark', '2019-12-25 12:22:59');
insert into Item_notes (item_ID, note, created_date) values (87, 'Intuitive content-based product', '2020-09-12 06:30:17');
insert into Item_notes (item_ID, note, created_date) values (10, 'Adaptive asynchronous architecture', '2020-03-17 11:03:37');
insert into Item_notes (item_ID, note, created_date) values (82, 'Horizontal asymmetric methodology', '2020-07-27 13:11:54');
insert into Item_notes (item_ID, note, created_date) values (51, 'Devolved 4th generation hardware', '2020-01-23 04:05:37');
insert into Item_notes (item_ID, note, created_date) values (82, 'Adaptive homogeneous concept', '2019-11-02 01:24:38');
insert into Item_notes (item_ID, note, created_date) values (24, 'Cloned 6th generation challenge', '2020-02-20 16:56:47');
insert into Item_notes (item_ID, note, created_date) values (65, 'Triple-buffered full-range standardization', '2019-12-06 08:11:32');
insert into Item_notes (item_ID, note, created_date) values (51, 'Mandatory composite interface', '2020-01-23 06:30:12');
insert into Item_notes (item_ID, note, created_date) values (32, 'Progressive explicit focus group', '2020-02-19 10:45:13');
insert into Item_notes (item_ID, note, created_date) values (26, 'Function-based client-server migration', '2019-10-28 13:04:29');
insert into Item_notes (item_ID, note, created_date) values (34, 'Function-based bandwidth-monitored open system', '2020-07-03 00:02:11');
insert into Item_notes (item_ID, note, created_date) values (46, 'Profit-focused maximized approach', '2020-02-20 00:29:33');
insert into Item_notes (item_ID, note, created_date) values (22, 'Decentralized bifurcated focus group', '2019-12-27 06:02:42');
insert into Item_notes (item_ID, note, created_date) values (100, 'Implemented object-oriented hub', '2020-01-29 05:11:16');
insert into Item_notes (item_ID, note, created_date) values (73, 'Digitized contextually-based synergy', '2020-07-27 21:39:00');
insert into Item_notes (item_ID, note, created_date) values (32, 'Mandatory fresh-thinking workforce', '2020-09-08 11:09:31');
insert into Item_notes (item_ID, note, created_date) values (99, 'User-centric leading edge challenge', '2020-03-02 22:30:41');
insert into Item_notes (item_ID, note, created_date) values (53, 'Versatile static array', '2020-09-12 00:29:41');
insert into Item_notes (item_ID, note, created_date) values (21, 'Function-based discrete benchmark', '2020-01-03 01:24:38');
insert into Item_notes (item_ID, note, created_date) values (21, 'Down-sized zero administration workforce', '2020-04-14 14:03:03');
";

                    SqliteCommand selectCommand = new SqliteCommand(insertScript, db);
                    selectCommand.ExecuteNonQuery();
                }
            }
        }

        public void EraseAllData()
        {
            if (tablesCreated)
            {
                using (SqliteConnection db =
                  new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();
                    string deleteScript = @"
DROP TABLE Item_notes;
DROP TABLE List_items;
DROP TABLE lists;";

                    SqliteCommand selectCommand = new SqliteCommand(deleteScript, db);
                    selectCommand.ExecuteNonQuery();
                }

                InitializeDatabase();
            }
        }

        private bool CheckIfListExistsByName(string listName)
        {
            if (tablesCreated)
            {
                using (SqliteConnection db =
                   new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();
                    string selectScript = "Select count(*) from Lists where list_name = @listName";


                    SqliteCommand selectCommand = new SqliteCommand(selectScript, db);
                    selectCommand.Parameters.AddWithValue("@listName", listName);

                    return (Int64)selectCommand.ExecuteScalar() == 1;
                }
            }
            return false;
        }
        private bool CheckIfListExistsByID(int listID)
        {
            if (tablesCreated)
            {
                using (SqliteConnection db =
                   new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();
                    string selectScript = "Select count(*) from Lists where id = @listID";


                    SqliteCommand selectCommand = new SqliteCommand(selectScript, db);
                    selectCommand.Parameters.AddWithValue("@listID", listID);

                    return (int)selectCommand.ExecuteScalar() == 1;
                }
            }
            return false;
        }

        public string AddNewListToTable(string listName)
        {
            if (CheckIfListExistsByName(listName))
            {
                return "A list by that name already exists.";
            }
            else if (tablesCreated)
            {
                using (SqliteConnection db =
                   new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();
                    string insertScript = "INSERT INTO Lists (list_name) VALUES (@listName)";

                    SqliteCommand insertStuff = new SqliteCommand(insertScript, db);
                    insertStuff.Parameters.AddWithValue("@listName", listName);

                    insertStuff.ExecuteNonQuery();

                    return "List created";
                }
            }

            return "Something went wrong";
        }

        public string AddNewItemToListItemTable(string itemName, int listID)
        {
            if (CheckIfListExistsByID(listID))
            {
                using (SqliteConnection db =
                   new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();
                    string insertScript = "INSERT INTO list_items (list_ID, item_name, is_complete) VALUES (@listID, @itemName, 0)";

                    SqliteCommand insertStuff = new SqliteCommand(insertScript, db);
                    insertStuff.Parameters.Add(new SqlParameter("@itemName", SqlDbType.Text).Value = itemName);
                    insertStuff.Parameters.Add(new SqlParameter("@listID", SqlDbType.Int).Value = listID);

                    insertStuff.ExecuteNonQuery();

                    return "Item Added";
                }
            }
            else
            {
                return "That list doesn't exist.";
            }
        }

        public string AddNewItemToListItemTableByListName(string itemName, string listName)
        {
            if (CheckIfListExistsByName(listName))
            {
                using (SqliteConnection db =
                   new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();
                    string insertScript = "INSERT INTO list_items (list_ID, item_name, is_complete) VALUES ((Select id from Lists where list_name = @listName), @itemName, 0)";

                    SqliteCommand insertStuff = new SqliteCommand(insertScript, db);
                    insertStuff.Parameters.AddWithValue("@itemName", itemName);
                    insertStuff.Parameters.AddWithValue("@listName", listName);

                    insertStuff.ExecuteNonQuery();

                    return "Item Added";
                }
            }
            else
            {
                return "That list doesn't exist.";
            }
        }

        private bool CheckIfItemExistsByID(int itemID)
        {
            if (tablesCreated)
            {
                using (SqliteConnection db =
                   new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();
                    string selectScript = "Select count(*) from Lists where id = @itemID";


                    SqliteCommand selectCommand = new SqliteCommand(selectScript, db);
                    selectCommand.Parameters.Add(new SqlParameter("@itemID", SqlDbType.Int).Value = itemID);

                    return (int)selectCommand.ExecuteScalar() == 1;
                }
            }
            return false;
        }

        public string AddNewNoteToItemTable(string noteText, int itemID)
        {
            return "";
        }

        public ObservableCollection<string> GetUserLists()
        {
            if (tablesCreated)
            {
                using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();
                    string selectScript = "Select list_name from Lists;";
                    SqliteCommand command = new SqliteCommand(selectScript, db);
                    SqliteDataReader reader = command.ExecuteReader();
                    ObservableCollection<string> result = new ObservableCollection<string>();
                    while (reader.Read())
                    {
                        result.Add(Convert.ToString(reader["list_name"]));
                    }
                    return result;
                }
            }
            return null;
        }

        public ObservableCollection<string> GetListItems(string listName)
        {
            if (tablesCreated)
            {
                using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();
                    string selectScript = "Select item_name from List_items where list_ID = (Select [id] from Lists where list_name = @name LIMIT 1);";
                    SqliteCommand command = new SqliteCommand(selectScript, db);
                    //command.Parameters.Add(new SqlParameter("@name", SqlDbType.Text).Value = listName);
                    command.Parameters.AddWithValue("@name", listName);
                    SqliteDataReader reader = command.ExecuteReader();
                    ObservableCollection<string> result = new ObservableCollection<string>();
                    while (reader.Read())
                    {
                        result.Add(Convert.ToString(reader["item_name"]));
                    }
                    return result;
                }
            }
            return null;
        }

    }
}
