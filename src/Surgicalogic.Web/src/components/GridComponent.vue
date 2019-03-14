<template>
  <v-card class="grid-card">
    <div class="v-card__title">
      <h2>
        {{ title }}
      </h2>

      <v-icon v-if="closable" @click="closeGrid" class="close-wrap">
              close
      </v-icon>
    </div>


    <v-card-title class="search-wrap" v-if="showSearch || !hideExport || showInsert">
      <v-text-field v-if="showSearch" v-model="search" append-icon="search" :label="$t('common.search')" v-on:keyup.enter="filterGrid" single-line hide-details>
      </v-text-field>

      <v-spacer></v-spacer>

      <v-btn v-if="!hideExport" class="orangeButton" @click="exportExcel">
        <v-icon color="white--text">
          arrow_downward
        </v-icon>
        {{ $t('common.export') }}
      </v-btn>

      <v-btn v-if="showInsert" class="orangeButton" slot="activator" @click="addNewItem">
        <v-icon color="white--text">
          add
        </v-icon>
        {{ $t('common.add') }}
      </v-btn>
    </v-card-title>
    <div class="v-card__text">
      <v-data-table id="section-to-print" :headers="headers" :items="items" :loading="loading" :pagination.sync="pagination" :total-items="totalCount" :hide-actions="hideActions" :rows-per-page-text="$t('common.rowsPerPage')" :no-data-text="$t('common.noDataAvailable')" :rows-per-page-items="[10, 25, 50, 100]">
        <v-progress-linear slot="progress" height="3" color="teal" indeterminate>
        </v-progress-linear>

        <template slot="items" slot-scope="props">
          <td v-for="(header, i) in headers" :key="i">
            <template v-if="!header.isAction && header.isCheck">
              <div v-if="props.item[header.value]">
                <v-icon class="green--text">check</v-icon>
              </div>

              <div v-else>
                <v-icon class="red--text">close</v-icon>
              </div>
            </template>

            <template v-else-if="!header.isAction && header.isDateTime">
              {{ props.item[header.value] | moment("DD.MM.YYYY HH:mm") }}
            </template>

             <template v-else-if="!header.isAction && header.overtimeValue">
               <span v-if="props.item[header.isOvertime]" class="overtimeMinutes"> {{ props.item[header.value] }} {{ $t('common.minute') }} </span>
               <span v-else class="belowtimeMinutes"> {{ props.item[header.value] }} {{ $t('common.minute') }} </span>
            </template>

            <template v-else-if="!header.isAction && header.isDate">
              {{ props.item[header.value] | moment("DD.MM.YYYY") }}
            </template>

            <template v-else-if="!header.isAction">
              {{ props.item[header.value] }}
            </template>

            <template v-else>
              <v-tooltip top>
                <v-btn v-if="showCalendar" slot="activator"  icon class="mx-0" @click="calendarItem(props.item)">
                <v-icon color="#232222">
                  date_range
                  </v-icon>
                </v-btn>
                <span>{{ $t('common.operatingRoomCalendar') }}</span>
              </v-tooltip>

              <v-tooltip top>
                <v-btn v-if="showPlanAnOperation" slot="activator" icon class="mx-0" @click="planAnOperation(props.item)">
                  <v-icon>
                    update
                  </v-icon>
                </v-btn>
                <span>{{ $t('common.planAnOperation') }}</span>
              </v-tooltip>

              <v-tooltip top>
                <v-btn v-if="showDetail" slot="activator" icon class="mx-0" @click="detailItem(props.item)">
                  <v-icon>
                    visibility
                  </v-icon>
                </v-btn>
                <span>{{ $t('common.viewRecord') }}</span>
              </v-tooltip>

              <v-tooltip top>
                <v-btn v-if="showEdit" slot="activator" icon class="mx-0" @click="editItem(props.item)">
                  <v-icon>
                    edit
                  </v-icon>
                </v-btn>
                <span>{{ $t('common.editRecord') }}</span>
              </v-tooltip>

              <v-tooltip top>
                <v-btn v-if="showDelete" slot="activator" icon class="mx-0" @click="deleteItem(props.item)">
                  <v-icon>
                    delete
                  </v-icon>
                </v-btn>
                <span>{{ $t('common.deleteRecord') }}</span>
              </v-tooltip>

              <v-btn v-if="showResetPassword" icon class="mx-0" @click="resetPassword(props.item)">
                <v-icon>
                  lock_open
                </v-icon>
              </v-btn>
            </template>
          </td>
        </template>
      </v-data-table>
    </div>
  </v-card>
</template>

<script>
export default {
    props: {
        items: {
            type: Array,
            required: false,
            default() {
                return [];
            }
        },

        loading: {
            type: Boolean,
            required: false
        },

        hideActions: {
            type: Boolean,
            required: false
        },

        headers: {
            type: Array,
            required: false
        },

        title: {
            type: String,
            required: false
        },

        showCalendar: {
            type: Boolean,
            required: false
        },

        showPlanAnOperation: {
            type: Boolean,
            required: false
        },

        showDetail: {
            type: Boolean,
            required: false
        },

        showEdit: {
            type: Boolean,
            required: false
        },

        showDelete: {
            type: Boolean,
            required: false
        },

        showSearch: {
            type: Boolean,
            required: false
        },

        showInsert: {
            type: Boolean,
            required: false
        },

        hideExport: {
            type: Boolean,
            required: false
        },

        showResetPassword: {
            type: Boolean,
            required: false
        },

        customParameters: {
            type: Object,
            required: false
        },

        closable:{
            type: Boolean,
            required: false
        },

        methodName: {
            type: Function,
            required: true
        },

        totalCount: {
            type: Number,
            required: false
        }
    },
    data() {
        return {
            search: '',
            dataRows: [],
            pagination: {}
        };
    },

    watch: {
        pagination: {
            handler() {
              const vm = this;

              vm.executeGridOperations();
            }
        }
    },

    methods: {
        addNewItem() {
            const vm = this;

            //When the add new button is clicked, the event is sent to the grid component
            vm.$emit('newaction');
        },

        exportExcel() {
            const vm = this;

            //When the add new button is clicked, the event is sent to the grid component
            vm.$emit('exportToExcel');
        },

        closeGrid() {
                      const vm = this;

            //When the add new button is clicked, the event is sent to the grid component
            vm.$emit('closeGrid');
        },

        filterGrid() {
            const vm = this;

            vm.executeGridOperations(true);
        },

        calendarItem(item) {
            const vm = this;

            //When the date range button is clicked, the event is sent to the grid component
            vm.$emit('calendar', item);
        },

        detailItem(item) {
            const vm = this;

            //When the detail button is clicked, the event is sent to the grid component
            vm.$emit('detail', item);
        },

        editItem(item) {
            const vm = this;

            //When the edit button is clicked, the event is sent to the grid component
            vm.$emit('edit', item, vm.items.indexOf(item));
        },

        planAnOperation(item){
            const vm = this;

            //When the date range button is clicked, the event is sent to the grid component
            vm.$emit('planAnOperation', item);
        },

        deleteItem(item) {
            const vm = this;

            //When the delete button is clicked, the event is sent to the grid component
            vm.$emit('deleteitem', item);
        },

        resetPassword(item) {
            const vm = this;

            //When the reset password button is clicked, the event is sent to the grid component
            vm.$emit('resetpassword', item);
        },

        executeGridOperations(resetPageCount) {
            const vm = this;

            if (resetPageCount) {
                vm.pagination.page = 1;
            }

            var params = {};

            if (vm.customParameters) {
                params = vm.customParameters;
            }

            const { sortBy, descending, page, rowsPerPage } = vm.pagination;

            (params.currentPage = page),
                (params.pageSize = rowsPerPage),
                (params.search = vm.search),
                (params.sortBy = vm.getSortByField(sortBy)),
                (params.descending = descending);

            vm.$store.dispatch(vm.methodName(), params);
        },

        getSortByField(sortBy) {
            const vm = this;

            vm.headers.forEach(element => {
                if (element.value == sortBy && element.sortBy) {
                    sortBy = element.sortBy;
                }
            });

            return sortBy;
        }
    }
};
</script>
