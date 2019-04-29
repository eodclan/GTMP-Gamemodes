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


$support = $_GET["support"];

############################################
///// Beitrag erstellen /////

if ($support == 'go_to_support') {

  //---- Create topic

$SocialClubName  = trim(htmlentities(urldecode($_POST['SocialClubName'])));
$CharName  = trim(htmlentities(urldecode($_POST['CharName'])));
$Vorfall = trim(htmlentities(urldecode($_POST['Vorfall'])));
$Supporter  = trim(htmlentities(urldecode($_POST['Supporter'])));
$Grund  = trim(htmlentities(urldecode($_POST['Grund'])));
$Datum = trim(htmlentities(urldecode($_POST['Datum'])));
$EintragungsDatum = trim(htmlentities(urldecode($_POST['EintragungsDatum'])));  
 
$data = array(
        "SocialClubName"   => $SocialClubName,
        "CharName" => $CharName,
        "Vorfall"  => $Vorfall,
        "Supporter"    => $Supporter,
        "Grund"      => $Grund,
        "Datum"      => $Datum,
        "EintragungsDatum"      => get_date_time()	
);
$db -> insertRow($data, staff_support);

stderr("Support System","<b><p>Der Vorfall wurde Erfolgreich eingetragen.<br><center><a href=\"?support=go_to_support_list\">Gehe in die Support Liste</a></p></b></center>");
}

$id = intval($_GET["Id"]);

$sql = "SELECT Id, SocialClubName, CharName, Vorfall, Supporter, Grund, Datum, EintragungsDatum FROM staff_support GROUP BY ID";
$res = $db -> queryObjectArray($sql);

x264_header("Support System");

check_access(UC_MODERATOR);

$trackerdienste = $GLOBALS["TORRENT_UPLOAD_OFF"];
if ($trackerdienste[0] == "0")
{
  stdmsg("Achtung","Das Support System ist momentan nicht möglich!");
  x264_footer();
  die();
}

echo"
<script type='text/javascript'>

setTimeout(function() {
  if (location.hash) {
    window.scrollTo(0, 0);
  }
}, 1);


</script>

<form method='post' action='?support=go_to_support' enctype='multipart/form-data'>	
					
                    <div class='row'>
                        <div class='col-lg-12'>
                            <div class='card'>
                                <div class='card-header'>
                                    <i class='fa fa-edit'></i>Support System - Vorfall Erstellen
                                    <div class='card-actions'>
                                        <a href='#' class='btn-close'><i class='icon-close'></i></a>
                                    </div>
                                </div>
                                <div class='card-block'>
								<h2>Charakter Daten</h2>
								
								Rockstar Social Club Benutzername
								<br />
								<div class='input-group mb-1'>
									<input type='text' name='SocialClubName' size='50' maxlength='60' class='form-control text-left btn btn-flat btn-primary fc-today-button'>
								</div>								
								
								Charakter Name
								<br />
								<div class='input-group mb-1'>
									<input type='text' name='CharName' size='50' maxlength='60' class='form-control text-left btn btn-flat btn-primary fc-today-button'>								
								</div>

								<h2>Der Vorfall - Bitte genau genug eintragen</h2>
								Vorfall
								<br />
								<div class='input-group mb-1'>
									<input type='text' name='Vorfall' size='50' maxlength='60' class='form-control text-left btn btn-flat btn-primary fc-today-button'>								
								</div>									
								
								Supporter Name
								<br />
								<div class='input-group mb-1'>
									<input type='text' name='Supporter' size='50' maxlength='60' class='form-control text-left btn btn-flat btn-primary fc-today-button'>								
								</div>
								
								Der Grund - Die Geschichte des Support Vorfalls
								<br />
								<div class='textarea-group mb-1'>
									<textarea type='text' name='Grund' size='350' maxlength='60' class='form-control text-left btn btn-flat btn-primary fc-today-button'></textarea>								
								</div>

								Datum des Vorfalls
								<br />
								<div class='input-group mb-1'>
									<input type='text' name='Datum' size='50' maxlength='60' class='form-control text-left btn btn-flat btn-primary fc-today-button'>								
								</div>	
								
								<div class='input-group mb-1'>
									<input type='submit' value='Eintragen' class='btn'>
								</div>
                                </div>
                            </div>
                        </div>
                        <!--/col-->
                    </div>
</form>
				
                    <div class='row'>
                        <div class='col-lg-12'>
                            <div class='card'>
                                <div class='card-header'>
                                    <i class='fa fa-gavel'></i> Support System - Vorfall
                                    <div class='card-actions'>
                                        <a href='#' class='btn-close'><i class='icon-close'></i></a>
                                    </div>
                                </div>								
					<div id='dataTables'>
									<table cellpadding='0' cellspacing='0' border='0' class='table table-bordered table-striped table-condensed'>
										<thead>
											<tr>										
												<th>Social Club Name</th>
												<th>Charakter Name</th>
												<th>Der Vorfall</th>
												<th>Supporter Name</th>
												<th>Der Grund</th>
												<th>Datum</th>
												<th>Eintragungsdatum</th>												
											</tr>
										</thead>
										<tbody>";
										
										if ($res)
										{
										foreach ($res as $arr)
										{

										echo"
											<tr>									
												<td>".$arr['SocialClubName']."</td>
												<td>".$arr['CharName']."</td>
												<td>".$arr['Vorfall']."</td>
												<td>".$arr['Supporter']."</td>
												<td>".$arr['Grund']."</td>
												<td>".$arr['Datum']."</td>
												<td>".$arr['EintragungsDatum']."</td>
											</tr>";
										}
										}
										else
										{
										echo"

										</tbody>
									</table>
									<table cellpadding='0' cellspacing='0' border='0' class='table table-bordered table-striped table-condensed'>
										<tbody>									
											<tr>									
												<td><i class='fa fa-warning text-red'></i> Es gibt momentan kein Support Vorfall.</td>
											</tr>";
										}
echo"										
										</tbody>
									</table>
					</div>
                                </div>
                            </div>
                        </div>
                    </div>";



x264_footer();
?>