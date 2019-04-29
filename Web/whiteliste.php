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
loggedinorreturn();
check_access(UC_MODERATOR);

$id = intval($_GET["Id"]);

$sql = "SELECT Id, SocialClubName, CreatedAt FROM whitelist GROUP BY ID";
$res = $db -> queryObjectArray($sql);

x264_header("Whitelist - Alle bisherigen Spieler");
?>
                    <div class='row'>
                        <div class='col-lg-12'>
                            <div class='card'>
                                <div class='card-header'>
                                    <i class='fa fa-gavel'></i> Whitelist - Alle bisherigen Spieler
                                    <div class='card-actions'>
                                        <a href='#' class='btn-close'><i class='icon-close'></i></a>
                                    </div>
                                </div>								
					<div id='dataTables'>
									<table cellpadding='0' cellspacing='0' border='0' class='table table-bordered table-striped table-condensed'>
										<thead>
											<tr>										
												<th>Social Club Name</th>
												<th>Datum & Uhrzeit</th>
											</tr>
										</thead>
										<tbody>
										<?php
										if ($res)
										{
										foreach ($res as $arr)
										{

										print "
											<tr>										
												<td>".$arr['SocialClubName']."</td>
												<td>".$arr['CreatedAt']."</td>
											</tr>";
										}
										}
										else
										{
										print "
											<tr>
												<td><i class='fa fa-warning text-red'></i></td>										
												<td>Momentan ist keiner in der Whitelist eingetragen.</td>
											</tr>";
										}
										?>
										</tbody>
									</table>
					</div>
                                </div>
                            </div>
                        </div>
                    </div>
<?php
x264_footer();
?>