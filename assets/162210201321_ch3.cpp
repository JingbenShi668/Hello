#include <iostream>
using namespace std;

struct Node
{
	int data;
	Node * next; 
	
}; 


class CircleList
{
	public :
		CircleList(int a[],int n);
		void EnQueqe(int x);
		int DeQueue(); 
		void show();
		
	private:
		Node * p;
		Node * rear;  
	 
};   //��������Ҫ�ӷֺ�  

CircleList::CircleList(int a[],int n)  //���в����Ĺ��캯�� ,β�巨����ѭ������ 
{	

    p = new Node;
	rear=p;  //βָ���ʼ�� 
 	for(int i=0;i<n;i++){
 		
 	    Node * s=new Node;	
 		s->data = a[i];
 		rear->next = s;
 	    rear=s; //���ڵ�s���뵽�ڵ�rear�ĺ��� 		
}	
   
       rear->next=p;
			 //��β�ڵ�ָ��ͷ�ڵ� 
		 		 	
}

void CircleList::EnQueqe(int x)   //��Ӳ�������β�ڵ�������� 
{
	Node * s;            //���г���ͷ�ڵ�ĵط���rear->next���� 
	s=new Node; 
	s->data = x;	
    s->next=rear->next;    //��β�ڵ㴦�����½ڵ� �����뵽β�ڵ���ͷ�ڵ�֮�䣬ѭ������ 
	rear->next=s;	
    rear=s;
		
}

int CircleList:: DeQueue()    //���Ӳ�������ͷ�ڵ�ɾ������ 
{   	
		Node * q;
		q=rear->next;
		int x=q->data;
		rear->next = q->next; //ժ�� 
		delete q;
		return x;
						  		   	     
}

void CircleList:: show()
{
	Node * s;
    s = rear->next->next;
    while(s->next!=rear->next->next)
    {
    	cout<< s->data <<" ";
	    s = s->next;
	}
    
}


int main(){
	int a[5]={1,9,3,4,8};
	CircleList list(a,5);
     
    cout<<"ԭʼ�Ķ���Ϊ��" <<endl;
    list.show();  
    cout<<endl;
    
    cout<<"��ɳ��Ӳ�����Ķ���Ϊ��"<<endl; 
	list.DeQueue();
    list.show(); 
    cout<<endl;
    
    cout<<"����ٴγ��Ӳ�����Ķ���Ϊ��"<<endl; 
	list.DeQueue();
    list.show(); 
    cout<<endl;
    
    cout<<"�����Ӳ�����Ķ���Ϊ��"<<endl; 
    list.EnQueqe(20);
    list.show();
    cout<<endl;
    
}
