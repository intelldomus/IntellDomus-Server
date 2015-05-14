<?php
	include("config.php");
	
	$temp = $_GET["temperatura"];
	$umidi = $_GET["umidita"];
	
	date_default_timezone_set("Europe/Rome");
	$query = "INSERT INTO rilevazioni (temperatura,umidita,dataOra) VALUES (".$temp.", ".$umidi.", \"".date('d-m-Y - H:i')."\")";
	
	mysqli_query($conn,$query) or die(mysqli_error($conn));
	
	echo "Query effettuata.";
	
	mysqli_close($conn);
?>