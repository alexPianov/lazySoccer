using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LazySoccer.Network.Error
{
    public class ErrorRequest : MonoBehaviour
    {
        public class DetailedError
        {
            public string message;
            public string stackTrace;
            public object innerException;
            public List<object> innerExceptions;
        }

        public class ValidationErrors
        {
            public List<ValidationError> validationErrors;
            public string message;
            public DetailedError detailedError;
        }

        public class ValidationError
        {
            public string title;
            public string description;
            public string field;
            public ErrorName titleEnum;
        }

        public class Errors
        {
            public List<string> Code;
            public string type;
            public string title;
            public int status;
            public string traceId;  
        }
        
        public class ServerError
        {
            public Errors errors;
            public string type;
            public string title;
            public int status;
            public string traceId;
        }
        
        public enum ErrorName
        {
            None,
            User_NOT_Found,
            Request_NOT_Found,
            Union_NOT_Found,
            PlayerTransfer_NOT_Found,
            Team_NOT_Found,
            Loot_NOT_Found,
            Box_NOT_Found,
            Game_NOT_Found,
            Skill_NOT_Found,
            Building_NOT_Found,
            Player_NOT_Found,
            Incorrect_token,
            Invalid_format_only_latin_characters_and_spaces,
            Invalid_format_only_latin_alphabetic_capital_characters,
            The_login_is_not_linked_to_any_account,
            The_account_is_not_confirmed,
            The_User_is_locked_out,
            Incorrect_password,
            The_email_is_already_occupied,
            Length_4_32_symbols,
            Length_3_symbols,
            This_user_name_contains_offensive_expressions,
            This_team_shortname_contains_offensive_expressions,
            This_team_name_contains_offensive_expressions,
            The_nickname_is_already_taken_please_create_a_new_one,
            The_team_name_is_already_taken_please_create_a_new_one,
            The_email_address_is_not_linked_to_any_account,
            The_token_is_locked_out,
            Negative_number,
            Not_enough_money,
            The_team_is_already_exists,
            The_wrong_player_list,
            Not_registered_nft,
            All_attempts_were_used,
            The_player_must_play_5_games_to_gain_access_to_the_traits,
            There_are_no_relevant_players,
            No_available_to_send_messages,
            The_email_is_already_confirmed,

            There_are_repeating_players,
            The_players_on_field_must_be_exactly_11,
            The_position_for_the_player_on_the_field_is_not_indicated,
            Unacceptable_line_ups_of_players,
            Such_positions_do_not_exist,
            No_Access,
            The_game_is_over_can_not_change_the_lineup,
            The_game_will_start_soon_can_not_change_the_lineup,
            The_players_do_not_belong_to_the_team,
            Invalid_PositionIndex,
            Invalid_Player_Status,
            Unacceptable_defensive_line_height_for_the_player,
            Unacceptable_mark_for_the_player,
            Unacceptable_player_in_goalkeeping_position,
            The_goalkeeper_can_only_be_on_the_gate,
            Unacceptable_game_time,
            The_game_tier_not_found,
            The_Host_team_is_already_playing_at_this_tima,
            The_Guest_team_is_already_playing_at_this_tima,
            The_building_is_improving,
            The_building_has_no_upgrades,
            The_building_has_no_downgardes,
            The_other_building_cannot_be_higher_level_than_the_office,
            The_limit_of_simultaneous_players_has_been_reached,
            The_limit_of_new_players_this_week_has_been_reached,
            The_limit_of_players_has_been_reached,
            Invalid_Loot_Status,
            The_Trade_chat_channel_is_not_unlocked_Communications_Center_be_10_lvl_or_more,
            The_limit_of_season_trade_offers_of_players_has_been_reached,
            The_player_must_be_at_100_physical_form,
            Job_Not_Found,
            The_price_is_lower_than_a_minimum_price,
            The_box_is_already_open,
            The_player_is_charged,
            It_is_impossible_while_team_is_a_member_of_another_union,
            The_request_has_already_been_sent,
            You_are_trying_to_sent_invite_yourself,
            Union_is_full,
            Member_not_found,
            Friend_not_found,
            Is_the_maximum,
            Choose_a_new_master,
            The_building_is_not_open_for_upgrade,
            Invalid_Building_Status,
            Need_more_gold,
            It_is_not_time_yet,
            Access_to_the_donation_of_this_building_has_already_begun,
            Access_to_the_donation_of_this_building_has_already_closed,
            The_amount_of_donation_exceeds_the_amount_needed_to_build,
            You_are_already_friends,
            Too_early_a_date,
            The_limit_of_friendly_matches_for_the_week_has_been_reached,
            The_limit_of_friendly_matches_for_the_week_for_friend_has_been_reached,
            Admin_not_found,
            The_manager_is_already_in_the_ban,
            The_manager_is_already_in_the_unban,
            Unknown_trait,
            Unknown_position,
            Too_yong,
            Too_old,
            Code_is_invalid_Remaining_attempts_before_lockout,
            The_country_name_is_already_taken_please_create_a_new_one,
            Country_not_found,
            Division_not_found,
            The_child_divisions_is_already_exists,
            The_child_divisions_is_not_exists,
            The_child_divisions_are_used,
            The_name_must_not_be_empty,
            The_box_name_is_already_occupied,
            The_lootbox_is_already_active,
            The_lootbox_is_already_inactive,
            The_box_is_already_active,
            The_box_is_already_inactive,
            The_name_is_already_occupied,
            Not_Found,
            Already_changed,
            Does_not_change_at_this_level,
            Incorrect_format,
            Unacceptable_Influence,
            Invalid_value,
            User_is_banned,
            The_limit_of_the_traits_has_been_reached,
            Must_contain_8_64_characters_including_letters_numbers_special_characters_no_spaces,
            Too_much_difference_in_power,
            minimum_contribution_value_must_be_no_less_than_limit,
            Not_enough_Teams
        }
    }
}
