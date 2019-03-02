function main (number_of_floor, number_of_elevator){
    let elevator_controller = new ElevatorController(number_of_floor, number_of_elevator);
    elevator_controller.elevator_list[0].currentFloor = 10;
    elevator_controller.elevator_list[0].direction = null;
    elevator_controller.elevator_list[0].status = "IDLE";
    elevator_controller.elevator_list[0].floorList = [];

    elevator_controller.elevator_list[1].currentFloor = 3;
    elevator_controller.elevator_list[1].direction = null;
    elevator_controller.elevator_list[1].status = "IDLE";
    elevator_controller.elevator_list[1].floorList = [];

    elevator_controller.requestElevator(10,"DOWN");
    elevator_controller.RequestFloor(1,3);
    elevator_controller.requestElevator(3,"DOWN");
    elevator_controller.RequestFloor(1,2);


}
class Elevator {
    constructor(number_of_elevator, number_of_floor){
        this.elevatorNumber = number_of_elevator;
        this.currentFloor = 1;
        this.direction = null;
        this.status = "idle";
        this.floorList = [];
        this.internalButton_list = [];
        for (let j = 1; j <= number_of_floor; j++){
            this.internalButton_list.push(new InternalButton(number_of_elevator, "off", j));
        }
       
        
    }   
}
class Button {
    constructor(direction, floor, status) {
        this.floor = floor;
        this.direction = direction;
        this.status = status;
    }
 }
 class InternalButton {
    constructor(number_of_elevator, status, floor){
    this.elevatorNumber = number_of_elevator;
    this.status = status;
    this.floor = floor;
    }
}
class ElevatorController {
    
    constructor(number_of_floor, number_of_elevator) {
 
        this.number_of_floor = number_of_floor;
        this.number_of_elevator = number_of_elevator;
        this.button_list = [];
        this.elevator_list = [];
            for (let i = 1; i < number_of_floor; i++) {
                this.button_list.push(new Button('UP', i, 'off'));
                this.button_list.push(new Button('DOWN', i + 1, 'off'));
            }
            for (let i = 1; i <= number_of_elevator; i++ ){
                this.elevator_list.push(new Elevator(i,number_of_floor));
            } 
            
    }
    requestElevator(floorNumber, direction) {             
      
     
         var Elevator = this.findElevator(floorNumber, direction, this.elevator_list);
        
         console.log("User is at floor ", floorNumber, "and wants to go ", direction);
         console.log("Elevator responding to the request:");
         console.log(Elevator);  
         
         
         this.bubbleSort(Elevator.floorList, Elevator.direction);
         
         this.operateElevator(Elevator, direction);
        
     }

    findElevator (floorNumber, direction, elevlist) {
            
            
            for(var i = 0; i < elevlist.length; i++){
                var e = elevlist[i];
                if(e.status === "STOPPED" && e.currentFloor === floorNumber && e.direction === direction){
                    e.floorList.push(floorNumber);
                    return e;
            
                }else if(e.status === "IDLE" && e.currentFloor === floorNumber){
                    e.floorList.push(floorNumber);
                    return e;
    
                }else if(e.currentFloor < floorNumber && (e.status === "MOVING" || "STOPPED") && e.direction === "UP" && direction === e.direction){  
                    e.floorList.push(floorNumber);
                    return e;
    
                }else if(e.currentFloor > floorNumber && (e.status === "MOVING" || "STOPPED") && e.direction === "DOWN" && direction === e.direction){
                    e.floorList.push(floorNumber);
                    return e;
                }else if(e.status === "IDLE"){
                    e.floorList.push(floorNumber);
                    return e;
                }

    bubbleSort(floorList, direction); { 
        if (direction === "UP"){
            var swapped;
            do {
                swapped = false;
                for (var i=0; i < floorList.length-1; i++) {
                    if (floorList[i] > floorList[i+1]) {
                        var temp = floorList[i];
                        floorList[i] = floorList[i+1];
                        floorList[i+1] = temp;
                        swapped = true;
                    }
                }
            } while (swapped);
        }

        else if(direction === "DOWN"){
            do {
                for (var i=0; i < floorList.length-1; i++) {
                    if (floorList[i] < floorList[i+1]) {
                        var temp = floorList[i];
                        floorList[i] = floorList[i+1];
                        floorList[i+1] = temp;
                    }
                }
            } while (swapped);
        }
        console.log("Sorted floorList : ", floorList);
        
    }
    
    operateElevator(Elevator, direction){
            console.log("next item on floor list : ", Elevator.floorList[0]);
            
        while (Elevator.floorList.length > 0){
                console.log("current floor : ", Elevator.currentFloor);
            if (Elevator.floorList[0] === Elevator.currentFloor){
                this.openDoor(Elevator, direction);
                Elevator.floorList.shift();
                console.log("new list : ",Elevator.floorList);
                console.log("next destination : ", Elevator.floorList[0]);
            }
            if (Elevator.floorList[0] > Elevator.currentFloor){
                this.moveUp(Elevator);
            }
            if (Elevator.floorList[0] < Elevator.currentFloor){
                this.moveDown(Elevator); 
            }
            
            
        }
            if (Elevator.floorList.length < 1){
                Elevator.status = "IDLE";
                Elevator.direction = null;
                console.log(Elevator);
            }
    }
    
    
    moveUp(Elevator); {
        if(Elevator.status =="IDLE" || Elevator.status =="STOPPED"){
            Elevator.status = "MOVING";
            Elevator.direction = "UP";
            console.log("Elevator start ", Elevator.status, " ", Elevator.direction);
        }
            Elevator.currentFloor++;
        
    }
    moveDown(Elevator); {
        if(Elevator.status =="IDLE" || Elevator.status =="STOPPED"){
        Elevator.status = "MOVING";
        Elevator.direction = "DOWN";
        console.log("elevator start moving", Elevator);
        }
        this.timer(2000);
        Elevator.currentFloor--;
       
    }
    openDoor(Elevator, direction); {
        if( Elevator.status === "IDLE"){
            Elevator.direction = direction;
        }
        Elevator.status = "STOPPED";
        
        console.log("Elevator is ",Elevator.status);
        this.timer(2000);
        console.log("DOOR OPENING");
        this.timer(2000);
        console.log("DOORS OPEN");
        this.timer(2000);
        this.closeDoor(ElevatSor);
    }

    closeDoor(Elevator);{
    console.log("DOOR IS CLOSING");
    this.timer(2000);
    console.log("DOOR IS CLOSE");
    }

    timer(milliseconds); {
        var start = new Date().getTime();
            if ((new Date().getTime() - start) > milliseconds) {
                break;
            }
        }
        
    };
            
        
                
    RequestFloor(Elevator, RequestedFloor);
        console.log("User is in elevator number ", Elevator, " and wants to go floor ", RequestedFloor);
        var Elevator =  this.interior_floorList(Elevator, RequestedFloor,this.elevator_list);
    
        this.bubbleSort(Elevator.floorList, Elevator.direction);
       
        this.operateElevator(Elevator, RequestedFloor);           
        interior_floorList(Elevator, RequestedFloor, elevatorList);
            for(var i = 0; i < elevatorList.length; i++){
                if( Elevator === elevatorList[i].elevatorNumber){
                    elevatorList[i].floorList.push(RequestedFloor);
                    return elevatorList[i];
                }
            }          
        }
