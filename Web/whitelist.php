<?php
// ************************************************************************************//
// * D€ Source 2018
// ************************************************************************************//
// * Author: D@rk-€vil™
// ************************************************************************************//
// * Version: 2.0
// * 
// * Copyright (c) 2017 - 2018 D@rk-€vil™. All rights reserved.
// ************************************************************************************//
// * License Typ: Creative Commons licenses
// ************************************************************************************//
// * Github: https://github.com/eodclan/x264_source
// ************************************************************************************// 
require_once(dirname(__FILE__) . "/include/bittorrent.php");
dbconn();

function getDatetimeNow() {
    $tz_object = new DateTimeZone('Brazil/East');
    //date_default_timezone_set('Brazil/East');

    $datetime = new DateTime();
    $datetime->setTimezone($tz_object);
    return $datetime->format('his');
}

$wlfactionlife = $_GET["wlfactionlife"];

############################################
///// Beitrag erstellen /////

if ($wlfactionlife == 'go_to_wlfactionlife') {

$SocialClubName  = trim(htmlentities(urldecode($_POST['SocialClubName'])));
  
$data = array(
        "SocialClubName"   => $SocialClubName
);
$db -> insertRow($data, whitelist);

stderr("Whitelist System","<b><p>Dein Whitelist wurde Erfolgreich erstellt.<br><center><a href=\"https://faction-life.ml\">Zur&uuml;ck zu den Forum</a></p></b></center>");
}

x264_header("Whitelist System");

check_access(UC_MODERATOR);

echo"

<form method='post' action='?wlfactionlife=go_to_wlfactionlife' enctype='multipart/form-data'>
                    <div class='row'>
                        <div class='col-lg-12'>
                            <div class='card'>
                                <div class='card-header'>
                                    <i class='fa fa-edit'></i>Whitelist System Info
                                    <div class='card-actions'>
                                        <a href='#' class='btn-close'><i class='icon-close'></i></a>
                                    </div>
                                </div>
                                <div class='card-block'>
									<a href='javascript:void(0)' class='uppercase'>Beachte bitte, dass du nachgefragt hast, ob der User noch nicht auf der Whitelist ist.</a>
                                </div>
                            </div>
                        </div>
                        <!--/col-->
                    </div>

                    <div class='row'>
                        <div class='col-lg-12'>
                            <div class='card'>
                                <div class='card-header'>
                                    <i class='fa fa-edit'></i>Whitelist System - Erstellen
                                    <div class='card-actions'>
                                        <a href='#' class='btn-close'><i class='icon-close'></i></a>
                                    </div>
                                </div>
                                <div class='card-block'>								
								Rockstar Social Club Benutzername
								<br />
								<div class='input-group mb-1'>
									<input type='text' name='SocialClubName' size='50' maxlength='60' class='form-control text-left btn btn-flat btn-primary fc-today-button'>
								</div>															
								<div class='input-group mb-1'>
									<input type='submit' value='Okay' class='btn'>
								</div>
                                </div>
                            </div>
                        </div>
                        <!--/col-->
                    </div>
</form>";

x264_footer();
?>