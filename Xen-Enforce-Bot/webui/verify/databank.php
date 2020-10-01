 <?php 

	if(!defined('USE_DATABASE')){ 

		echo '<img src="dbnoaccess.jpg" style="width:100%;height:100%">';
		die(); 

	}

	$host = "";
	$user = "";
	$password = "";
	$dbdatabase = "";
	$dbtable = "";




	if (!function_exists("resultToArray")) {
		function resultToArray($result) {
		    $rows = array();
		    while($row = $result->fetch_assoc()) {
		        $rows[] = $row;
		    }
		    return $rows;
		}
	}






	$DB_OBJ = new mysqli($host, $user, $password, $dbdatabase)
		or die('Failed to connect: ' . mysqli_error());
		mysqli_set_charset($DB_OBJ, "utf8");
	if ($DB_OBJ->connect_errno) {
			echo "Database connection failed";
			exit();
	}
?>