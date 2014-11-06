--0704
insert into CM_USER values('area2','HIb01XUByvxvVpK9S++rpw==',null,null,GETDATE(),2,1);
insert into CM_USER_AREA values(10859,'0704',1,null);
--0705
insert into CM_USER values('area3','9KF+fsRvpEyDf8mSAjsEfQ==',null,null,GETDATE(),2,1);
insert into CM_USER_AREA values(10860,'0705',1,null);
--0706
insert into CM_USER values('area4','vQCR31z9CtzPjsdn8zAtyw==',null,null,GETDATE(),2,1);
insert into CM_USER_AREA values(10861,'0706',1,null);
--0707
insert into CM_USER values('area5','tUhPqh/Qr7jJC3Es4ssPgg==',null,null,GETDATE(),2,1);
insert into CM_USER_AREA values(10862,'0707',1,null);
--0708
insert into CM_USER values('area6','N6cE/7mWDMI2tiGYLdbyBg==',null,null,GETDATE(),2,1);
insert into CM_USER_AREA values(10863,'0708',1,null);

select * from cm_user order by user_id desc;
select * from CM_USER_AREA order by AREA_CODE;

