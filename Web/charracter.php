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


$factionlife = $_GET["factionlife"];

############################################
///// Beitrag erstellen /////

if ($factionlife == 'go_to_factionlife') {

  //---- Create topic

$SocialClubName  = trim(htmlentities(urldecode($_POST['SocialClubName'])));
$FirstName  = trim(htmlentities(urldecode($_POST['FirstName'])));
$LastName = trim(htmlentities(urldecode($_POST['LastName'])));
$Gender  = trim(htmlentities(urldecode($_POST['Gender'])));
$Hair  = trim(htmlentities(urldecode($_POST['Hair'])));
$HairColor = trim(htmlentities(urldecode($_POST['HairColor']))); 
$HairHighlightColor  = trim(htmlentities(urldecode($_POST['HairHighlightColor'])));
$Father  = trim(htmlentities(urldecode($_POST['Father'])));
$Mother = trim(htmlentities(urldecode($_POST['Mother'])));
$EyeColor  = trim(htmlentities(urldecode($_POST['EyeColor'])));
  
$data = array(
        "SocialClubName"   => $SocialClubName,
        "CreatedAt" => get_date_time(),
        "FirstName" => $FirstName,
        "LastName"  => $LastName,
        "Gender"    => $Gender,
        "Hair"      => $Hair,
        "HairColor"      => $HairColor,
        "HairHighlightColor"      => $HairHighlightColor,
        "Father"      => $Father,
        "Mother"      => $Mother,
        "EyeColor"      => $EyeColor,
        "Cash"      => 500,
        "Bank"      => 1500,
        "Phone"      => 3513671 + getDatetimeNow(),
        "ClothesTop"      => 91,
        "ClothesLegs"      => 1,
        "ClothesFeets"      => 1,
        "ClothesMasks"      => 1,
        "ClothesDecals"      => 1,
        "ClothesHair"      => $Hair,
        "ClothesBackpacks"      => 1,
        "ClothesAccessories"      => 1,
        "ClothesUndershirt"      => 1,
        "ClothesTorso"      => 1,
        "Bought_Clothing"      => '{}',
        "Maskstate"      => 1,
        "Hunger"      => 100,        
        "Thirst"      => 100		
);
$db -> insertRow($data, characters);

stderr("Charakter Erstellung","<b><p>Dein Charakter wurde Erfolgreich erstellt.<br><center><a href=\"index.php\">Zur&uuml;ck zu den Forum</a></p></b></center>");
}

x264_header("Charakter erstellen");

$trackerdienste = $GLOBALS["TORRENT_UPLOAD_OFF"];
if ($trackerdienste[0] == "0")
{
  stdmsg("Achtung","Die Character Erstellung ist momentan nicht möglich!");
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

                    <div class='row'>
                        <div class='col-lg-12'>
                            <div class='card'>
                                <div class='card-header'>
                                    <i class='fa fa-edit'></i>Voice Info
                                    <div class='card-actions'>
                                        <a href='#' class='btn-close'><i class='icon-close'></i></a>
                                    </div>
                                </div>
                                <div class='card-block'>
									<a href='javascript:void(0)' class='uppercase'>Denk daran, dass du in unseren Teamspeak 3 Server sein musst, wenn du auf die Insel willst!</a>
                                </div>
                            </div>
                        </div>
                        <!--/col-->
                    </div>


                    <div class='row'>
                        <div class='col-lg-12'>
                            <div class='card'>
                                <div class='card-header'>
                                    <i class='fa fa-edit'></i>Charakter Info
                                    <div class='card-actions'>
                                        <a href='#' class='btn-close'><i class='icon-close'></i></a>
                                    </div>
                                </div>
                                <div class='card-block'>
									<a href='javascript:void(0)' class='uppercase'>Beachte bitte, dass du ein 2. Charakter im Forum beantragen musst!</a>
                                </div>
                            </div>
                        </div>
                        <!--/col-->
                    </div>

					<div class='row'>
                        <div class='col-lg-12'>
                            <div class='card'>
                                <div class='card-header'>
                                    <i class='fa fa-download'></i> Charakter Aussehen - Bitte benutzen!
                                    <div class='card-actions'>
                                        <a href='#' class='btn-close'><i class='icon-close'></i></a>
                                    </div>
                                </div>
                                <div class='card-block'>
									<table class='table table-bordered table-striped table-condensed'>
										<thead>
											<tr>
												<td>
															<h2>Mutter / Vater Aussehen</h2>
															<p>
																Bitte die Nummern benutzen!
															</p>												
													<div class='postthumb'>
														<a href='design/ex1080_default/img/face.jpg' data-lightbox='face'><img src='design/ex1080_default/img/face.jpg' alt='Bitte anklicken'></a>
														<div class='title'>
														</div>
													</div>												
												</td>
												<td>
															<h2>Frauen Haar Schnitt</h2>
															<p>
																Bitte die Nummern benutzen!
															</p>												
													<div class='postthumb'>
														<a href='design/ex1080_default/img/frauhaare.jpg' data-lightbox='frauhaare'><img src='design/ex1080_default/img/frauhaare.jpg' alt='Bitte anklicken'></a>
														<div class='title'>
														</div>
													</div>												
												</td>
												<td>
															<h2>Man Haar Schnitt</h2>
															<p>
																Bitte die Nummern benutzen!
															</p>												
													<div class='postthumb'>
														<a href='design/ex1080_default/img/manhaare.jpg' data-lightbox='manhaare'><img src='design/ex1080_default/img/manhaare.jpg' alt='Bitte anklicken'></a>
													</div>												
												</td>
												<td>
															<h2>Farben</h2>
															<p>
																Kann für alle Farben benutzt werden!
															</p>												
													<div class='postthumb'>
														<a href='design/ex1080_default/img/haarfarbe.jpg' data-lightbox='haarfarbe'><img src='design/ex1080_default/img/haarfarbe.jpg' alt='Bitte anklicken'></a>
													</div>
														Bitte die Nummern benutzen!
												</td>												
											</tr>
										</thead>
									</table>
                                </div>
                            </div>
                        </div>
                        <!--/col-->
                    </div>	


<form method='post' action='?factionlife=go_to_factionlife' enctype='multipart/form-data'>	
					
                    <div class='row'>
                        <div class='col-lg-12'>
                            <div class='card'>
                                <div class='card-header'>
                                    <i class='fa fa-edit'></i>Charakter Erstellen - Du musst dich vorher Whitelisten lassen!
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
								
								Vorname
								<br />
								<div class='input-group mb-1'>
									<input type='text' name='FirstName' size='50' maxlength='60' class='form-control text-left btn btn-flat btn-primary fc-today-button'>								
								</div>

								Nachname
								<br />
								<div class='input-group mb-1'>
									<input type='text' name='LastName' size='50' maxlength='60' class='form-control text-left btn btn-flat btn-primary fc-today-button'>								
								</div>									

								<h2>Charakter Aussehen</h2>								
								Geschlecht - 0 = Man - 1 = Frau
								<br />
								<div class='input-group mb-1'>
									<select name='Gender' class='form-control text-left btn btn-flat btn-primary fc-today-button'>
										<option>0</option>
										<option>1</option>			
									</select>																
								</div>
								
								Haar Schnitt
								<br />
								<div class='input-group mb-1'>
									<select name='Hair' class='form-control text-left btn btn-flat btn-primary fc-today-button'>
										<option>0</option>
										<option>1</option>
										<option>2</option>
										<option>3</option>
										<option>4</option>
										<option>5</option>
										<option>6</option>
										<option>7</option>
										<option>8</option>
										<option>9</option>
										<option>10</option>
										<option>11</option>
										<option>12</option>
										<option>13</option>
										<option>14</option>
										<option>15</option>
										<option>16</option>
										<option>17</option>
										<option>18</option>
										<option>19</option>
										<option>20</option>
										<option>21</option>
										<option>22</option>
										<option>23</option>
										<option>24</option>
										<option>25</option>
										<option>26</option>
										<option>27</option>
										<option>28</option>
										<option>29</option>			
									</select>
								</div>

								Haar Farbe
								<br />
								<div class='input-group mb-1'>
									<select name='HairColor' class='form-control text-left btn btn-flat btn-primary fc-today-button'>
										<option>0</option>
										<option>1</option>
										<option>2</option>
										<option>3</option>
										<option>4</option>
										<option>5</option>
										<option>6</option>
										<option>7</option>
										<option>8</option>
										<option>9</option>
										<option>10</option>
										<option>11</option>
										<option>12</option>
										<option>13</option>
										<option>14</option>
										<option>15</option>
										<option>16</option>
										<option>17</option>
										<option>18</option>
										<option>19</option>
										<option>20</option>
										<option>21</option>
										<option>22</option>
										<option>23</option>
										<option>24</option>
										<option>25</option>
										<option>26</option>
										<option>27</option>
										<option>28</option>
										<option>29</option>
										<option>30</option>
										<option>31</option>
										<option>32</option>
										<option>33</option>
										<option>34</option>
										<option>35</option>
										<option>36</option>
										<option>37</option>
										<option>38</option>
										<option>39</option>
										<option>40</option>
										<option>41</option>
										<option>42</option>
										<option>43</option>
										<option>44</option>
										<option>45</option>
										<option>46</option>
										<option>47</option>
										<option>48</option>
										<option>50</option>
										<option>51</option>
										<option>52</option>
										<option>53</option>
										<option>54</option>
										<option>55</option>
										<option>56</option>
										<option>57</option>
										<option>58</option>
										<option>59</option>
										<option>60</option>
										<option>61</option>
										<option>62</option>											
									</select>								
								</div>

								Haar Highlight Farbe
								<br />
								<div class='input-group mb-1'>
									<select name='HairHighlightColor' class='form-control text-left btn btn-flat btn-primary fc-today-button'>
										<option>0</option>
										<option>1</option>
										<option>2</option>
										<option>3</option>
										<option>4</option>
										<option>5</option>
										<option>6</option>
										<option>7</option>
										<option>8</option>
										<option>9</option>
										<option>10</option>
										<option>11</option>
										<option>12</option>
										<option>13</option>
										<option>14</option>
										<option>15</option>
										<option>16</option>
										<option>17</option>
										<option>18</option>
										<option>19</option>
										<option>20</option>
										<option>21</option>
										<option>22</option>
										<option>23</option>
										<option>24</option>
										<option>25</option>
										<option>26</option>
										<option>27</option>
										<option>28</option>
										<option>29</option>
										<option>30</option>
										<option>31</option>
										<option>32</option>
										<option>33</option>
										<option>34</option>
										<option>35</option>
										<option>36</option>
										<option>37</option>
										<option>38</option>
										<option>39</option>
										<option>40</option>
										<option>41</option>
										<option>42</option>
										<option>43</option>
										<option>44</option>
										<option>45</option>
										<option>46</option>
										<option>47</option>
										<option>48</option>
										<option>50</option>
										<option>51</option>
										<option>52</option>
										<option>53</option>
										<option>54</option>
										<option>55</option>
										<option>56</option>
										<option>57</option>
										<option>58</option>
										<option>59</option>
										<option>60</option>
										<option>61</option>
										<option>62</option>											
									</select>								
								</div>									
								
								Augen Farbe
								<br />
								<div class='input-group mb-1'>
									<select name='EyeColor' class='form-control text-left btn btn-flat btn-primary fc-today-button'>
										<option>0</option>
										<option>1</option>
										<option>2</option>
										<option>3</option>
										<option>4</option>
										<option>5</option>
										<option>6</option>
										<option>7</option>
										<option>8</option>
										<option>9</option>
										<option>10</option>
										<option>11</option>
										<option>12</option>
										<option>13</option>
										<option>14</option>
										<option>15</option>
										<option>16</option>
										<option>17</option>
										<option>18</option>
										<option>19</option>
										<option>20</option>
										<option>21</option>
										<option>22</option>
										<option>23</option>
										<option>24</option>
										<option>25</option>
										<option>26</option>
										<option>27</option>
										<option>28</option>
										<option>29</option>
										<option>30</option>
										<option>31</option>
										<option>32</option>
										<option>33</option>
										<option>34</option>
										<option>35</option>
										<option>36</option>
										<option>37</option>
										<option>38</option>
										<option>39</option>
										<option>40</option>
										<option>41</option>
										<option>42</option>
										<option>43</option>
										<option>44</option>
										<option>45</option>
										<option>46</option>
										<option>47</option>
										<option>48</option>
										<option>50</option>
										<option>51</option>
										<option>52</option>
										<option>53</option>
										<option>54</option>
										<option>55</option>
										<option>56</option>
										<option>57</option>
										<option>58</option>
										<option>59</option>
										<option>60</option>
										<option>61</option>
										<option>62</option>											
									</select>								
								</div>

								<h2>Charakter Ähnlichkeit</h2>									
								Mutter
								<br />
								<div class='input-group mb-1'>
									<select name='Mother' class='form-control text-left btn btn-flat btn-primary fc-today-button'>
										<option>0</option>
										<option>1</option>
										<option>2</option>
										<option>3</option>
										<option>4</option>
										<option>5</option>
										<option>6</option>
										<option>7</option>
										<option>8</option>
										<option>9</option>
										<option>10</option>
										<option>11</option>
										<option>12</option>
										<option>13</option>
										<option>14</option>
										<option>15</option>
										<option>16</option>
										<option>17</option>
										<option>18</option>
										<option>19</option>
										<option>20</option>
										<option>21</option>
										<option>22</option>
										<option>23</option>
										<option>24</option>
										<option>25</option>
										<option>26</option>
										<option>27</option>
										<option>28</option>
										<option>29</option>
										<option>30</option>
										<option>31</option>
										<option>32</option>
										<option>33</option>
										<option>34</option>
										<option>35</option>
										<option>36</option>
										<option>37</option>
										<option>38</option>
										<option>39</option>
										<option>40</option>
										<option>41</option>
										<option>42</option>
										<option>43</option>											
									</select>								
								</div>

								Vater
								<br />
								<div class='input-group mb-1'>
									<select name='Father' class='form-control text-left btn btn-flat btn-primary fc-today-button'>
										<option>0</option>
										<option>1</option>
										<option>2</option>
										<option>3</option>
										<option>4</option>
										<option>5</option>
										<option>6</option>
										<option>7</option>
										<option>8</option>
										<option>9</option>
										<option>10</option>
										<option>11</option>
										<option>12</option>
										<option>13</option>
										<option>14</option>
										<option>15</option>
										<option>16</option>
										<option>17</option>
										<option>18</option>
										<option>19</option>
										<option>20</option>
										<option>21</option>
										<option>22</option>
										<option>23</option>
										<option>24</option>
										<option>25</option>
										<option>26</option>
										<option>27</option>
										<option>28</option>
										<option>29</option>
										<option>30</option>
										<option>31</option>
										<option>32</option>
										<option>33</option>
										<option>34</option>
										<option>35</option>
										<option>36</option>
										<option>37</option>
										<option>38</option>
										<option>39</option>
										<option>40</option>
										<option>41</option>
										<option>42</option>
										<option>43</option>			
									</select>								
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