
select * from application;

select * from application where id_application = 2;

select * from application where name_client like "%9%";

select count(*) from application where status = "выполнено";

select * from application;
select sec_to_time(avg(time_to_sec(time_work))) as avgtime from application where status = "выполнено";

select type_problem.name, count(*) 
from application
join type_problem on application.id_type_problem = type_problem.id_type_problemt
group by type_problem.name;

-- select type_problem.name, count(*) 
-- from application
-- join type_problem
-- where application.id_type_problem = type_problem.id_type_problemt
-- group by type_problem.name;

insert into application (date_addition, name_equipment, id_type_problem, name_client, period_execution, number, description, id_type_equipment) 
values(current_date(), "чайник Tefal", 1, "Иванов И.И.", "2024-02-17", 123456789, "Сломана крышка чайника", 1);