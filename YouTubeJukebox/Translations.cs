using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace YouTubeJukebox
{
    /// <summary>
    /// Allow to get the whole app in English or French
    /// </summary>

    public static class Translations
    {
        private static Dictionary<string, string> translations;

        /// <summary>
        /// Return a tranlation for the requested text
        /// </summary>
        /// <param name="msg_name">text identifier</param>
        /// <returns>returns translation for this identifier</returns>
        public static string Get(string msg_name)
        {
            if (translations.ContainsKey(msg_name))
                return translations[msg_name];

            return msg_name.ToUpper();
        }

        /// <summary>
        /// Initialize translations to French or English depending on system language
        /// </summary>
        static Translations()
        {
            translations = new Dictionary<string, string>();
            
            if (CultureInfo.CurrentCulture.ThreeLetterISOLanguageName == "fra")
            {
                translations["text_loading"] = "Chargement...";
                translations["text_videos"] = "Vidéos : ";
                translations["text_retrieving"] = "Récupération des vidéos pour '{0}'...";
                translations["text_playback_issues"] = "Problèmes de lecture ?";

                translations["error_invalid_data"] = "Des données invalides ont été reçues";
                translations["error_other"] = "Une erreur est survenue";
                translations["error_loading"] = "Erreur de chargement des vidéos";
                translations["error_network"] = "Une erreur réseau est survenue";
                translations["error_while_loading"] = " lors du chargement ";
                translations["error_incomplete_playlist"] = "des vidéos.\nLa playliste sera donc incomplète.";
                translations["error_of_webpage"] = "de la page.";
                translations["error_unknown_arg"] = "Argument '{0}' non reconnu.";

                translations["help_command"] = "Usage : {0} <chaîne YouTube> [sortie=stdio] [args]";
                translations["help_channel"] = "Chaîne YouTube : Nom ou lien de la chaîne YouTube";
                translations["help_output"] = "Sortie : Fichier de sortie, sinon la sortie standard sera utilisée";
                translations["help_reverse"] = "--reverse ou -r : Lit en commençant par la vidéo la plus ancienne";
                translations["help_shuffle"] = "--shuffle ou -s : Lit les vidéos dans un ordre aléatoire";
                translations["help_onlynew"] = "--onlynew ou -o : Lit seulement les nouvelles vidéos (inutile si -n)";
                translations["help_nocache"] = "--nocache ou -n : Ne crée/utilise pas le fichier de cache des vidéos";
                translations["help_verbose"] = "--verbose ou -v : Affiche l'avancement de la récupération des vidéos";
                translations["help_help"] = "--help    ou -? : Affiche ce message d'aide";
                translations["help_gui"] = "--gui     ou -g : Lance l'interface graphique";

                translations["player"] = "Lecteur de Médias";
                translations["player_select"] = "Sélectionner un lecteur de médias";
                translations["player_prompt"] = "Veuillez sélectionner un lecteur de médias";
                translations["player_filter_exe"] = "Fichier Exécutable";
                translations["player_upgrade"] = "Mettre à jour le script YouTube de VLC";
                translations["player_upgrading"] = "Mise à jour du script YouTube...";
                translations["player_upgrade_done"] = "Le script a été mis à jour avec succès.";
                translations["player_upgrade_error"] = "Une erreur est survenue lors de la mise à jour du script.";
                translations["player_upgrade_not_found"] = "Le lecteur VLC n'a pas été trouvé dans le répertoire spécifié.";
                translations["player_upgrade_up_to_date"] = "Le script est déjà à jour.";

                translations["tab_youtube"] = "YouTube";
                translations["tab_player"] = "Lecteur";
                translations["tab_about"] = "A Propos...";

                translations["setting_shuffle"] = "Mélanger les vidéos";
                translations["setting_reverse"] = "Lire en ordre inverse";
                translations["setting_onlynew"] = "Seulement les nouvelles vidéos";

                translations["button_play"] = "Lecture";
                translations["button_exit"] = "Quitter";
                translations["button_cancel"] = "Annuler";

                translations["about_title"] = Program.Name;
                translations["about_subtitle"] = "Version " + Program.Version + " - Par ORelio";
                translations["about_description"] = "Lecture facile de chaînes YouTube\r\nInspiré par youtube.nestharion.de";
                translations["about_visit_me"] = "Voir mon profil GitHub";
            }
            else
            {
                translations["text_loading"] = "Loading...";
                translations["text_videos"] = "Videos: ";
                translations["text_retrieving"] = "Retrieving videos for '{0}'...";
                translations["text_playback_issues"] = "Playback issues?";

                translations["help_command"] = "Usage: {0} <YouTube channel> [output=stdio] [args]";
                translations["help_channel"] = "YouTube channel: Username or Url to the YouTube channel";
                translations["help_output"] = "Output : Output file, otherwise standard output will be used";
                translations["help_reverse"] = "--reverse or -r : Play oldest video first instead of newest first";
                translations["help_shuffle"] = "--shuffle or -s : Play videos in a random order (overrides -r)";
                translations["help_onlynew"] = "--onlynew or -o : Play only new videos (has no effect if using -n)";
                translations["help_nocache"] = "--nocache or -n : Prevent video cache file from being used or created";
                translations["help_verbose"] = "--verbose or -v : Show status messages while retrieving videos";
                translations["help_help"] = "--help    or -? : Shows this help message";
                translations["help_gui"] = "--gui     or -g : Run in graphical mode";

                translations["error_invalid_data"] = "Got invalid data";
                translations["error_other"] = "An error occured";
                translations["error_loading"] = "Video loading error";
                translations["error_network"] = "A network error occured";
                translations["error_while_loading"] = " while loading ";
                translations["error_incomplete_playlist"] = "videos.\nThe playlist will be incomplete.";
                translations["error_of_webpage"] = "of the webpage.";
                translations["error_unknown_arg"] = "Unknown argument '{0}'.";

                translations["player"] = "Media Player";
                translations["player_select"] = "Select a media player";
                translations["player_prompt"] = "Please select a media player";
                translations["player_filter_exe"] = "Executable File";
                translations["player_upgrade"] = "Auto-Upgrade VLC YouTube Script";
                translations["player_upgrading"] = "Upgrading YouTube Script...";
                translations["player_upgrade_done"] = "The script has been successfully updated.";
                translations["player_upgrade_error"] = "An error occured while upgrading the script.";
                translations["player_upgrade_not_found"] = "The VLC Media Player could not be found in the specified location.";
                translations["player_upgrade_up_to_date"] = "The script is already up-to-date.";

                translations["tab_youtube"] = "YouTube";
                translations["tab_player"] = "Player";
                translations["tab_about"] = "About...";

                translations["setting_shuffle"] = "Shuffle videos";
                translations["setting_reverse"] = "Reverse order";
                translations["setting_onlynew"] = "Only new videos";

                translations["button_play"] = "Play";
                translations["button_exit"] = "Exit";
                translations["button_cancel"] = "Cancel";

                translations["about_title"] = Program.Name;
                translations["about_subtitle"] = "Version " + Program.Version + " - By ORelio";
                translations["about_description"] = "Easy playback of YouTube Channels\r\nInspired by youtube.nestharion.de";
                translations["about_visit_me"] = "Visit my GitHub profile";
            }
        }
    }
}
